using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Interfaces;

namespace EcommerceRV.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : MainController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return Ok(_productService.FindById(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);

                return Ok();
            }
            catch (Exception exception)
            {
                throw new ApplicationException("El producto no puede ser eliminado por que contiene informacion relacionada con el sistema");
            }
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ICollection<Product>> Get([FromQuery] ProductRequestParameters query)
        {
            return Ok(_productService.ListProducts(query));
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Product> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            var newProduct = _productService.AddProduct(product);

            return Created("api/products", newProduct);
        }

        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Product> Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }
            _productService.UpdateProduct(product);

            return Ok(product);
        }
    }
}
