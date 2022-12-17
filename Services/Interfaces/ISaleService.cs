using Entities;
using Entities.RequestParameters;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISaleService
    {
        Sale AddSale(Sale sale);
        void UpdateSale(Sale sale);
        void CancelSale(Sale sale);
        ICollection<Sale> ListSales(SaleRequestParameters query);
        Sale FindById(int id);
        string GetSaleCode();
    }
}
