using DataAccess.Interfaces;
using Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class StockService : IStockService
    {
        private readonly IRepository<Stock> stockRepository;

        public StockService(
            IRepository<Stock> stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        public Stock AddStock(Stock stock)
        {
            var newStock = stockRepository.Add(stock);

            return newStock; 
        }

        public Stock FindById(int id)
        {
            var stock = stockRepository.FindById(id);

            return stock;
        }

        public ICollection<Stock> ListStocks()
        {
            var stocks = stockRepository.List;

            return stocks.ToList();
        }        

        public void UpdateStock(Stock stock)
        {
            var currentStock = FindById(stock.Id);

            if (currentStock != null)
            {
                if (stock.Quantity >= 0)
                {
                    stockRepository.Update(stock);
                }
                else
                {
                    throw new ApplicationException("Stock insuficiente");
                }
            }
        }
    }
}
