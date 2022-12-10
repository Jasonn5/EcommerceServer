using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class OrderDetail : Entity
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitaryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public Order Order { get; set; }
    }
}
