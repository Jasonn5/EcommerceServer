using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceRV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ICollection<Order>> Get()
        {
            return Ok(orderService.list());
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Order> Post(Order order)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            order.OrderDate = TimeZoneHelper.GetSaWesternStandardTime();
            var newOrder = orderService.Add(order);

            return Created("api/orders", newOrder);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Order> Get(int id)
        {
            return Ok(orderService.FindById(id));
        }

        [HttpPatch]
        [Route("{id}")]
        public ActionResult<Order> Update(Order order)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            var updatedOrder = orderService.Update(order);

            return Ok(updatedOrder);
        }

    }
}
