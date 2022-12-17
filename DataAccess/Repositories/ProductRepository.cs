using DataAccess.Interfaces;
using DataAccess.Model.Product;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IdentityDbContext _dataAccess;

        public ProductRepository(IdentityDbContext dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IQueryable<Product> List
        {
            get
            {
                return _dataAccess.Set<Product>();
            }
        }

        public Product Add(Product product)
        {           
            _dataAccess.Set<Product>().Add(product);
            _dataAccess.SaveChanges();

            return product;
        }

        public void Delete(Product product)
        {
            _dataAccess.Set<Product>().Remove(product);
            _dataAccess.SaveChanges();
        }

        public IEnumerable<Product> GetProducts(string search)
        {
            var products = _dataAccess.Set<ProductGetView>().FromSqlRaw($"dbo.GetProducts '{search}'").AsEnumerable();

            return products
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.ProductName,
                    Price = p.Price,
                }).ToList();
        }

        public void AddRange(IEnumerable<Product> products)
        {
            _dataAccess.AddRange(products);
            _dataAccess.SaveChanges();
        }

        public Product FindById(int id)
        {
            return _dataAccess.Set<Product>()
                .SingleOrDefault(p => p.Id == id);
        }

        public IQueryable<Product> ListWithInclude<TInclude>(Expression<Func<Product, TInclude>> includeFunc) where TInclude : Entity
        {
            return _dataAccess.Set<Product>().Include(includeFunc);
        }

        public IQueryable<Product> ListWithInclude<TInclude1, TInclude2>(Expression<Func<Product, TInclude1>> includeFunc1, Expression<Func<Product, TInclude2>> includeFunc2)
            where TInclude1 : Entity
            where TInclude2 : Entity
        {
            return _dataAccess.Set<Product>().Include(includeFunc1).Include(includeFunc2);
        }

        public void Update(Product product)
        {
            var currentProduct = _dataAccess.Set<Product>()
                .SingleOrDefault(p => p.Id == product.Id);

            currentProduct.Name = product.Name;
            currentProduct.Description = product.Description;
            currentProduct.Price = product.Price;
            currentProduct.StockAlarm = product.StockAlarm;
            _dataAccess.SaveChanges();
        }
    }
}
