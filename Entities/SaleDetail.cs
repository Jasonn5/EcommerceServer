using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class SaleDetail : Entity
    {
        public int StockId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitaryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public Sale Sale { get; set; }
    }
}
