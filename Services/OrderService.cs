using Authentication.Entities;
using DataAccess.Interfaces;
using Entities;
using Entities.Enums;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) 
        {
            _orderRepository = orderRepository;
        }

        public Order Add(Order order)
        {
            order.StatusId = (int)OrderStatusEnum.Pending;

            return _orderRepository.Add(order);
        }

        public Order FindById(int id)
        {
            var order = _orderRepository.FindById(id);

            return order;
        }
        public ICollection<Order> list()
        {
            var orders = _orderRepository.list();
                
            return orders.ToList();
        }

        public Order Update(Order order)
        {
            try
            {             
                _orderRepository.Update(order);

                return order;
            }
            catch (Exception) 
            { 
               throw new NotImplementedException();
            }
        }
    }
}
