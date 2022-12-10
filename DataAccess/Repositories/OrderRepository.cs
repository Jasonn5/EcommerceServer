using Authentication.Entities;
using DataAccess.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IdentityDbContext dataAccess;

        public OrderRepository(IdentityDbContext dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public Order Add(Order entity)
        {
            if (entity.Client != null)
            {
                var client = dataAccess.Set<User>().Find(entity.Client.Id);

                if (client != null)
                {
                    entity.Client = client;
                }
            }

            if (entity.OrderDetails != null)
            {
                var orderDetails = new List<OrderDetail>();
                foreach (var od in entity.OrderDetails)
                {
                    od.Id = 0;
                    orderDetails.Add(od);
                }
                entity.OrderDetails = orderDetails;
            }

            dataAccess.Set<Order>().Add(entity);
            dataAccess.SaveChanges();

            return entity;
        }

        public Order FindById(int id)
        {
            return dataAccess.Set<Order>()
                .Include(o => o.Client)
                .Include(o => o.OrderDetails)
                .SingleOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> list()
        {
            return dataAccess.Set<Order>();
        }

        public void Update(Order entity)
        {
            var order = dataAccess.Set<Order>()
                .Include(s => s.OrderDetails)
                .Include(s => s.Client)
                .SingleOrDefault(s => s.Id == entity.Id);

            order.StatusId = entity.StatusId;
            dataAccess.SaveChanges();
        }
    }
}
