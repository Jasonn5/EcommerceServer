using Entities;
using Entities.RequestParameters;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IStockService
    {
        Stock AddStock(Stock stock);
        ICollection<Stock> ListStocks();
        void UpdateStock(Stock stock);
        Stock FindById(int id);
    }
}
