using DataAccess.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class StockRepository : IRepository<Stock>
    {
        private readonly IdentityDbContext dataAccess;

        public StockRepository(IdentityDbContext dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IQueryable<Stock> List
        {
            get
            {
                return this.dataAccess.Set<Stock>().Include(s => s.Product);
            }
        }

        public Stock Add(Stock entity)
        {
            if (entity.Product != null)
            {
                var product = dataAccess.Set<Product>().Find(entity.Product.Id);

                if (product != null)
                {
                    entity.Product = product;
                }
            }

            dataAccess.Set<Stock>().Add(entity);
            dataAccess.SaveChanges();

            return entity;
        }

        public void Delete(Stock entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Stock> FilterList(Expression<Func<Stock, bool>> filterFunc)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Stock> FilterListWithInclude<TInclude>(Expression<Func<Stock, bool>> filterFunc, Expression<Func<Stock, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Stock> FilterListWithIncludeArray<TInclude>(Expression<Func<Stock, bool>> filterFunc, Expression<Func<Stock, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public Stock FindById(int id)
        {
            return dataAccess.Set<Stock>()
                .Include(s => s.Product)
                .SingleOrDefault(s => s.Id == id);
        }

        public Stock FindByIdWithInclude<TInclude>(int id, Expression<Func<Stock, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public Stock FindByIdWithIncludeArray<TInclude>(int id, Expression<Func<Stock, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Stock> ListWithInclude<TInclude>(Expression<Func<Stock, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Stock> ListWithInclude<TInclude1, TInclude2>(Expression<Func<Stock, TInclude1>> includeFunc1, Expression<Func<Stock, TInclude2>> includeFunc2)
            where TInclude1 : Entity
            where TInclude2 : Entity
        {
            return dataAccess.Set<Stock>().Include(includeFunc1).Include(includeFunc2);
        }

        public IQueryable<Stock> ListWithIncludeArray<TInclude>(Expression<Func<Stock, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public void Update(Stock entity)
        {
            var stock = dataAccess.Set<Stock>().Find(entity.Id);

            if (entity.Product != null)
            {
                var product = dataAccess.Set<Product>().Find(entity.Product.Id);

                if (product != null)
                {
                    stock.Product = product;
                }
            }

            stock.Quantity = entity.Quantity;
            dataAccess.SaveChanges();
        }
    }
}
