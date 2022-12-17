using DataAccess.Model;
using Entities;
using System;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        public Order Add(Order entity);
        public Order FindById(int id);
        public void Update(Order entity);
        IEnumerable<Order> list();
        IEnumerable<OrderOrderDetails> GetOrders(DateTime startDate, DateTime endDate);
    }
}
