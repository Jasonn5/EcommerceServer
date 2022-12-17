using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Product Add(Product product);
        void AddRange(IEnumerable<Product> products);
        void Delete(Product product);
        void Update(Product entity);
        Product FindById(int id);
        IQueryable<Product> List { get; }
        IEnumerable<Product> GetProducts(string search);
        IQueryable<Product> ListWithInclude<TInclude>(Expression<Func<Product, TInclude>> includeFunc) where TInclude : Entity;
        IQueryable<Product> ListWithInclude<TInclude1, TInclude2>(Expression<Func<Product, TInclude1>> includeFunc1, Expression<Func<Product, TInclude2>> includeFunc2)
            where TInclude1 : Entity
            where TInclude2 : Entity;
    }
}
