using DataAccess.Interfaces;
using DataAccess.Interfaces.Custom;
using Entities;
using Entities.Enums;
using Entities.RequestParameters;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class SaleService : ISaleService
    {
        private readonly IRepository<Sale> saleRepository;
        private readonly IRepository<Stock> stockRepository;
        private readonly ISaleRepository customSaleRepository;
        private readonly IStockService stockService;

        public SaleService(
            IRepository<Sale> saleRepository,
            IRepository<Stock> stockRepository,
            ISaleRepository customSaleRepository,
            IStockService stockService)
        {
            this.saleRepository = saleRepository;
            this.stockRepository = stockRepository;
            this.customSaleRepository = customSaleRepository;
            this.stockService = stockService;
        }

        public Sale AddSale(Sale sale)
        {
            sale.Code = GetSaleCode();
            sale.StatusId = (int)SaleStatusEnum.Active;

            decimal total = 0;

            foreach (var saleDetail in sale.SaleDetails)
            {
                if (saleDetail.StockId != 0)
                {
                    var stock = stockService.FindById(saleDetail.StockId);
                    if (stock.Quantity >= saleDetail.Quantity)
                    {
                        stock.Quantity = stock.Quantity - saleDetail.Quantity;
                        stockRepository.Update(stock);
                    }
                    else
                    {
                        throw new ApplicationException("Stock insuficiente");
                    }
                }
            }

            total = sale.SaleDetails.Sum(sd => sd.TotalPrice);
            var newSale = saleRepository.Add(sale);

            return newSale;
        }

        public Sale FindById(int id)
        {
            var sale = customSaleRepository.GetSaleById(id)
                .GroupBy(s => s.Id)
                .Select(sr => new Sale
                {
                    Id = sr.First().Id,
                    Code = sr.First().Code,
                    Date = sr.First().SaleDate,
                    Description = sr.First().Description,
                    StatusId = sr.First().StatusId,
                    SaleDetails = sr.Select(sd => new SaleDetail
                    {
                        Id = sd.SaleDetailId,
                        StockId = sd.StockId,
                        ProductName = sd.ProductName,
                        Quantity = sd.Quantity,
                        UnitaryPrice = sd.UnitaryPrice,
                        TotalPrice = sd.TotalPrice,
                    }).OrderBy(sd => sd.ProductName).ToList()
                });

            return sale.SingleOrDefault();
        }

        public ICollection<Sale> ListSales(SaleRequestParameters query)
        {
            var sales = customSaleRepository.GetSales(query.StartDate, query.EndDate, query.Search)
                .GroupBy(s => s.Id)
                .Select(sg => new Sale
                {
                    Id = sg.First().Id,
                    Code = sg.First().Code,
                    Date = sg.First().SaleDate,
                    Description = sg.First().Description,
                    StatusId = sg.First().StatusId,
                    SaleDetails = sg.Select(sd => new SaleDetail
                    {
                        Id = sd.SaleDetailId,
                        TotalPrice = sd.TotalPrice
                    }).ToList()
                });

            sales = sales.OrderByDescending(s => s.Code);

            return sales.ToList();
        }

        public string GetSaleCode()
        {
            var lastSale = saleRepository.List.OrderByDescending(s => s.Id).FirstOrDefault();

            if (lastSale != null)
            {
                var code = lastSale.Id + 1 < 10000 ? (lastSale.Id + 1).ToString("D4") : (lastSale.Id + 1).ToString();

                return code;
            }
            else
            {
                return "0001";
            }
        }

        public void UpdateSale(Sale sale)
        {
            var currentSale = saleRepository.FindByIdWithIncludeArray(sale.Id, s => s.SaleDetails);
            var refreshStockMovements = false;
            var changeDate = currentSale.Date.Date != sale.Date.Date;

            foreach (var sd in sale.SaleDetails)
            {
                if (sd.StockId != 0)
                {
                    var currentStock = stockService.FindById(sd.StockId);

                    if (sd.Id < 0)
                    {
                        currentStock.Quantity -= sd.Quantity;
                        stockRepository.Update(currentStock);
                        refreshStockMovements = true;
                    }
                    else
                    {
                        var currentSaleDetail = currentSale.SaleDetails.SingleOrDefault(csd => csd.Id == sd.Id);

                        if (sd.Quantity != currentSaleDetail.Quantity)
                        {
                            currentStock.Quantity += currentSaleDetail.Quantity;
                            currentStock.Quantity -= sd.Quantity;
                            refreshStockMovements = true;

                            stockRepository.Update(currentStock);
                        }
                    }
                }
            }

            foreach (var csd in currentSale.SaleDetails)
            {
                if (csd.StockId != 0)
                {
                    var stock = stockService.FindById(csd.StockId);

                    if (!sale.SaleDetails.Any(sd => sd.Id == csd.Id))
                    {
                        stock.Quantity += csd.Quantity;
                        stockRepository.Update(stock);
                        refreshStockMovements = true;
                    }
                }
            }

            decimal newTotal = sale.SaleDetails.Sum(x => x.TotalPrice);
            decimal currentTotal = currentSale.SaleDetails.Sum(x => x.TotalPrice);

            saleRepository.Update(sale);

        }

        public void CancelSale(Sale sale)
        {
            var currentSale = saleRepository.FindByIdWithIncludeArray(sale.Id, s => s.SaleDetails);

            foreach (var sd in currentSale.SaleDetails)
            {
                var currentStock = stockService.FindById(sd.StockId);
                currentStock.Quantity += sd.Quantity;
                stockRepository.Update(currentStock);
            }

            currentSale.StatusId = (int)SaleStatusEnum.Inactive;
            saleRepository.Update(currentSale);
        }

    }
}