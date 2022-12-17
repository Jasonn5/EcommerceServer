using DataAccess.Interfaces;
using Entities;
using Entities.Enums;
using Entities.Reports;
using Entities.RequestParameters;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Reports.Services.Interfaces;
using Services.Helpers;
using Services.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Stock> _stockRepository;
        private readonly ISaleService _saleService;
        private readonly ITemplateService _templateService;
        private readonly IPdfFileGenerator _pdfFileGenerator;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        public OrderService(
            IRepository<Stock> stockRepository,
            ISaleService saleService,
            ITemplateService templateService,
            IOrderRepository orderRepository,
            IPdfFileGenerator pdfFileGenerator,
            IConfiguration configuration)
        {
            _stockRepository = stockRepository;
            _saleService = saleService;
            _templateService = templateService;
            _orderRepository = orderRepository;
            _pdfFileGenerator = pdfFileGenerator;
            _configuration = configuration;
        }

        public Order AddOrder(Order order)
        {
            order.Code = GetOrdereCode();
            order.StatusId = (int)OrderStatusEnum.Pending;

            return _orderRepository.Add(order);
        }

        public string GetOrdereCode()
        {
            var lastOrder = _orderRepository.list().OrderByDescending(o => o.Id).FirstOrDefault();

            if (lastOrder != null)
            {
                var code = lastOrder.Id + 1 < 10000 ? (lastOrder.Id + 1).ToString("D4") : (lastOrder.Id + 1).ToString();

                return code;
            }
            else
            {
                return "0001";
            }
        }

        public ICollection<Order> ListOrders(OrderRequestParameters query)
        {
            var orders = _orderRepository.GetOrders(query.StartDate, query.EndDate)
                .GroupBy(o => o.Id)
                .Select(sg => new Order
                {
                    Id = sg.First().Id,
                    Code = sg.First().Code,
                    OrderDate = sg.First().OrderDate,
                    StatusId = sg.First().StatusId,
                    OrderDetails = sg.Select(ord => new OrderDetail
                    {
                        Id = ord.OrderDetailId,
                        StockId = ord.StockId,
                        Quantity = ord.Quantity,
                        UnitaryPrice = ord.UnitaryPrice,
                        TotalPrice = ord.TotalPrice
                    }).ToList()
                });

            if (query != null)
            {
                if (query.StatusId != 0)
                {
                    orders = orders.Where(o => o.StatusId == query.StatusId);
                }

                if (query.Search != "" && query.Search != null)
                {
                    orders = orders.Where(
                    o => o.Code.Contains(query.Search));
                }
            }

            orders = orders.OrderByDescending(o => o.OrderDate);

            return orders.ToList();
        }

        public Order UpdateStatusOrder(Order order)
        {
            if (order.StatusId == (int)OrderStatusEnum.Completed)
            {               
                order.CompletedDate = TimeZoneHelper.GetSaWesternStandardTime();

                List<SaleDetail> saleDetails = new List<SaleDetail>();
                var newSale = new Sale
                {
                    Date = order.CompletedDate.Value,
                    Description = "Venta por Orders a Cliente " ,
                    StatusId = (int)SaleStatusEnum.Active,
                };

                foreach (var orderDetail in order.OrderDetails)
                {
                    var stock = _stockRepository.FindById(orderDetail.StockId);
                    var newSaleDetail = new SaleDetail
                    {
                        StockId = orderDetail.StockId,
                        ProductName = stock.Product.Name,
                        Quantity = orderDetail.Quantity,
                        UnitaryPrice = orderDetail.UnitaryPrice,
                        TotalPrice = orderDetail.TotalPrice,
                        Sale = newSale
                    };
                    saleDetails.Add(newSaleDetail);
                }

                newSale.SaleDetails = saleDetails;
                var sale = _saleService.AddSale(newSale);
                order.Sale = sale;
            }

            _orderRepository.Update(order);

            return order;
        }

        public byte[] generatePreOrder(Order order)
        {
            var htmlBody = "";
            var orderDetails = new List<PrintPreOrder>();
            decimal total = 0;

            foreach (var orderDetail in order.OrderDetails)
            {
                var orderDetailsToPrint = new PrintPreOrder
                {
                    Quantity = orderDetail.Quantity,
                    ProductName = orderDetail.ProductName,
                    UnitaryPrice = orderDetail.UnitaryPrice,
                    TotalPrice = orderDetail.TotalPrice
                };

                orderDetails.Add(orderDetailsToPrint);
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                total += orderDetail.TotalPrice;
            }


            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText("wwwroot/Templates/Order/Pre-order.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            htmlBody = builder.HtmlBody
                .Replace("#Logo#", _configuration["LogoUrl"])
                .Replace("#PrintDate#", order.OrderDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture))
                .Replace("#TotalAmount#", total.ToString())
                .Replace("#literalAmount#", Converter.NumbersToLetters(total));

            htmlBody = _templateService.GenerateTableTemplate<PrintPreOrder>(htmlBody, orderDetails);

            return _pdfFileGenerator.GeneratePdf(htmlBody);
        }

        public Order FindById(int id)
        {
            var order = _orderRepository.FindById(id);

            return order;
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.Update(order);
        }
    }
}
