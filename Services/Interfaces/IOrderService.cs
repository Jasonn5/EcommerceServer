using Entities;
using Entities.RequestParameters;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        ICollection<Order> ListOrders(OrderRequestParameters query);
        Order AddOrder(Order order);
        Order UpdateStatusOrder(Order order);
        void UpdateOrder(Order order);
        Order FindById(int id);
        byte[] generatePreOrder(Order order);
    }
}