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
    public class SalesController : MainController
    {
        private readonly ISaleService saleService;

        public SalesController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Sale> Post(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            sale.Date = sale.Date.Date + TimeZoneHelper.GetSaWesternStandardTime().TimeOfDay;
            var newSale = saleService.AddSale(sale);

            return Created("api/sales", newSale);
        }

        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Sale> Update(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }
            sale.Date = sale.Date.Date + TimeZoneHelper.GetSaWesternStandardTime().TimeOfDay;
            saleService.UpdateSale(sale);

            return Ok(sale);
        }

        [HttpPatch]
        [Route("cancelSale/sales/{id}")]
        public ActionResult<Sale> CancelSale(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            saleService.CancelSale(sale);

            return Ok(sale);
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ICollection<Sale>> Get([FromQuery] SaleRequestParameters query)
        {
            return Ok(saleService.ListSales(query));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Sale> Get(int id)
        {
            return Ok(saleService.FindById(id));
        }

        [HttpGet]
        [Route("getSaleCode/{date}")]
        public ActionResult<Sale> GetSaleCode(string date)
        {
            var saleCode = saleService.GetSaleCode();

            return new Sale
            {
                Code = saleCode
            };
        }
    }
}
