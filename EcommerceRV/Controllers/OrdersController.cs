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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : MainController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ICollection<Order>> Get([FromQuery] OrderRequestParameters query)
        {
            return Ok(orderService.ListOrders(query));
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
            var newOrder = orderService.AddOrder(order);

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

            var updatedOrder = orderService.UpdateStatusOrder(order);

            return Ok(updatedOrder);
        }

        [HttpPost]
        [Route("generate-pre-order")]
        public IActionResult Get(Order order)
        {
            var preOrderFile = orderService.generatePreOrder(order);

            return File(preOrderFile, "application/pdf", "pre_order.pdf"); ;
        }

        [HttpPatch]
        [Route("update-order/orders/{id}")]
        public ActionResult<Order> UpdateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            orderService.UpdateOrder(order);

            return Ok(order);
        }
    }
}
