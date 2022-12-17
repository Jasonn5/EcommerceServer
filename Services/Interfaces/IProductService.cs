using Entities;
using Entities.RequestParameters;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IProductService
    {
        ICollection<Product> ListProducts(ProductRequestParameters query);
        Product AddProduct(Product product);
        void AddProducts(List<Product> products);
        void DeleteProduct(int id);
        Product FindById(int id);
        void UpdateProduct(Product product);
    }
}
