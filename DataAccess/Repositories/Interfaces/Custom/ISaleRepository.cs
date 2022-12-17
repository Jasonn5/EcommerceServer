using DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DataAccess.Interfaces.Custom
{
    public interface ISaleRepository
    {
        IEnumerable<SaleSaleDetails> GetSales(DateTime startDate, DateTime endDate, string search);
        IEnumerable<SaleSaleDetailsCreditClientSeller> GetSaleById(int saleId);
    }
}
