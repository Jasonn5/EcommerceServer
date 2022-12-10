using Authentication.Entities;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class Order : Entity
    {
        public string Code { get; set; }

        public int OrderNumber { get; set; }

        public int StatusId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public User Client { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
