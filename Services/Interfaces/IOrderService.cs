using Entities;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        public Order Add(Order order);
        public Order FindById(int id);
        public Order Update(Order order);
        ICollection<Order> list();
    }
}
