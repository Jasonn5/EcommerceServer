using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Stock : Entity
    {
        public decimal Quantity { get; set; }

        [NotMapped]
        public decimal Price { get; set; }

        public Product Product { get; set; }
    }
}
