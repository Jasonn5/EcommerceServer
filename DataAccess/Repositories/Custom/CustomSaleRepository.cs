using DataAccess.Interfaces.Custom;
using DataAccess.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories.Custom
{
    public class CustomSaleRepository : ISaleRepository
    {
        private readonly IdentityDbContext dataAccess;

        public CustomSaleRepository(IdentityDbContext dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<SaleSaleDetailsCreditClientSeller> GetSaleById(int saleId)
        {
            var sale = dataAccess.Set<SaleSaleDetailsCreditClientSeller>().FromSqlRaw($"dbo.GetSale '{saleId}'").AsNoTracking().AsEnumerable();

            return sale;
        }

        public IEnumerable<SaleSaleDetails> GetSales(DateTime startDate, DateTime endDate, string search)
        {
            var sales = dataAccess.Set<SaleSaleDetails>().FromSqlRaw($"dbo.GetSales '{startDate.ToString("MM/dd/yyyy")}', '{endDate.ToString("MM/dd/yyyy 23:59:59")}', '{search}'").AsNoTracking().AsEnumerable();

            return sales;
        }
    }
}
