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
    public class StocksController : MainController
    {
        private readonly IStockService stockService;

        public StocksController(IStockService stockService)
        {
            this.stockService = stockService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            return Ok(stockService.ListStocks());
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Stock> Get(int id)
        {
            return Ok(stockService.FindById(id));
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Stock> Post(Stock stock)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }
            var newStock = stockService.AddStock(stock);

            return Created("api/stocks", newStock);
        }

        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Stock> Update(Stock stock)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }
            stockService.UpdateStock(stock);

            return Ok(stock);
        }
    }
}
