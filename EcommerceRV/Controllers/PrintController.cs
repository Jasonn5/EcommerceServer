using Entities.Enums;
using Entities.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace EcommerceRV.Controllers
{
    [Authorize]
    [Route("api/print")]
    [ApiController]
    public class PrintController : MainController
    {
        private readonly IReportService _printReceiptService;
        public PrintController(
            IReportService printReceiptService) 
        {
            _printReceiptService = printReceiptService;
        }


        [HttpGet]
        [Route("printReceipt/{saleId}/{isCopy}")]
        public IActionResult PrintReceipt(int saleId, bool isCopy)
        {
            byte[] file = null;
            file = _printReceiptService.PrintLetterReceipt(saleId);

            return File(file, "application/pdf", "sale_receipt.pdf"); ;
        }

        [HttpPost]
        [Route("report")]
        public ActionResult Post(ReportParams parameters)
        {
            byte[] reportFile = null;
            reportFile = _printReceiptService.PrintSalesReport(parameters);


            return File(reportFile, "application/pdf", "SaleDetail_Report.pdf");
        }
    }
}
