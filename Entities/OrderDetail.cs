using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class OrderDetail : Entity
    {
        public int StockId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitaryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public Order Order { get; set; }
    }
}
