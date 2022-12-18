using Entities.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IReportService
    {
        byte[] PrintLetterReceipt(int saleId);
        byte[] PrintSalesReport(ReportParams parameters);
    }
}
