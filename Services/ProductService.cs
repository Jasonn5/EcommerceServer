using DataAccess.Interfaces;
using Entities;
using Entities.RequestParameters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Stock> stockRepository;
        private readonly IProductRepository _productRepository;

        public ProductService(IRepository<Stock> stockRepository, IProductRepository productRepository)
        {
            this.stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public Product AddProduct(Product product)
        {
            return _productRepository.Add(product);
        }

        public void AddProducts(List<Product> products)
        {
            _productRepository.AddRange(products);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.FindById(id);

            if (product != null)
            {
                try
                {
                    _productRepository.Delete(product);
                }
                catch
                {
                    throw new ApplicationException("No se puede borrar el producto ya que tiene stock asociado.");
                }
            }
        }

        public Product FindById(int id)
        {
            var product = _productRepository.FindById(id);

            return product;
        }

        public ICollection<Product> ListProducts(ProductRequestParameters query)
        {
            var products = _productRepository.GetProducts(query.Search);

            products = products.OrderBy(p => p.Name);

            return products.ToList();
        }

        public void UpdateProduct(Product product)
        {
            var companyProduct = _productRepository.FindById(product.Id);

            if (companyProduct != null)
            {
                _productRepository.Update(product);
            }
        }
    }
}
