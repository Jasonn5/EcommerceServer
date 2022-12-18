using Entities.Reports;
using Entities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Reports.Services.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Reports.Services;
using Services.Helpers;
using DataAccess.Interfaces;
using System.Linq;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IPdfFileGenerator pdfFileGenerator;
        private readonly ITemplateService templateService;
        private readonly ISaleService saleService;
        private readonly IStockService stockService;
        private readonly IConfiguration _configuration;

        public ReportService(IPdfFileGenerator pdfFileGenerator,
            ISaleService saleService,
            IStockService stockService,
            ITemplateService templateService, 
            IRepository<Sale> saleRepository,
            IConfiguration configuration)
        {
            this.pdfFileGenerator = pdfFileGenerator;
            this.templateService = templateService;
            this.saleService = saleService;
            this.stockService = stockService;
            _configuration = configuration;
            _saleRepository= saleRepository;
        }

        public byte[] PrintLetterReceipt(int saleId)
        {
            var sale = saleService.FindById(saleId);
            var htmlBody = "";
            var saleDetails = new List<PrintSale>();
            decimal total = 0;
            decimal quantity = 0;

            foreach (var saleDetail in sale.SaleDetails)
            {
                total += saleDetail.TotalPrice;
                quantity += saleDetail.Quantity;
            }

            foreach (var saleDetail in sale.SaleDetails)
            {
                var saleDetailsToPrint = new PrintSale
                {
                    Code = sale.Code,
                    ProductName = saleDetail.ProductName,
                    Quantity = saleDetail.Quantity,
                    UnitaryPrice = saleDetail.UnitaryPrice,
                    TotalPrice = saleDetail.TotalPrice
                };

                saleDetails.Add(saleDetailsToPrint);
            }

            var title = "Nota de venta";
            var builder = new BodyBuilder();

            using (StreamReader SourceReader = File.OpenText("wwwroot/Templates/Sale/Receip.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            htmlBody = builder.HtmlBody
                .Replace("#Logo#", _configuration["LogoUrl"])
                .Replace("#Title#", title)
                .Replace("#saleCode#", sale.Code)
                .Replace("#saleDate#", sale.Date.ToString("MMMM dd, yyyy", new CultureInfo("es-ES")))
                .Replace("#literalAmount#", Converter.NumbersToLetters(total))
                .Replace("#totalAmount#", total.ToString());

            htmlBody = templateService.GenerateTableTemplate<PrintSale>(htmlBody, saleDetails);

            return pdfFileGenerator.GeneratePdf(htmlBody);
        }

        public byte[] PrintSalesReport(ReportParams parameters)
        {
            var sales = _saleRepository.List
                .Where(s => s.Date.Date >= parameters.StartDate.Date && s.Date.Date <= parameters.EndDate.Date).ToList();

            var SalesbyDate = sales
                .Select(s => new SaleReport
                {
                    Date = s.Date.ToString("dd/MM/yyyy"),
                    Code = s.Code,
                    Description = s.Description,
                    SubTotal = s.SaleDetails.Sum(sd => sd.TotalPrice)
                }).ToList();

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText("wwwroot/Templates/Reports/SaleReport.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string htmlBody = builder.HtmlBody
                .Replace("#startDate#", parameters.StartDate.ToString("dd/MM/yyyy"))
                .Replace("#endDate#", parameters.EndDate.ToString("dd/MM/yyyy"))
                .Replace("#currentDate#", TimeZoneHelper.GetSaWesternStandardTime().ToString("dd/MM/yyyy"))
                .Replace("#Total#", String.Format("{0:n}", sales.Sum(s => s.SaleDetails.Sum(sd => sd.TotalPrice))));

            htmlBody = templateService.GenerateTableTemplate(htmlBody, SalesbyDate);

            return pdfFileGenerator.GeneratePdf(htmlBody);
        }
    }
}
