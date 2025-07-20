using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(string id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            var createdOrder = _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public ActionResult<Order> UpdateOrder(string id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var updatedOrder = _orderService.UpdateOrder(order);
            if (updatedOrder == null)
            {
                return NotFound();
            }

            return updatedOrder;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(string id)
        {
            var success = _orderService.DeleteOrder(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpPut("{id}/shipping-address")]
        public IActionResult UpdateShippingAddress(string id, [FromBody] ShippingAddress shippingAddress)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            order.ShippingAddress = shippingAddress;
            var updatedOrder = _orderService.UpdateOrder(order);

            return Ok(updatedOrder);
        }
    }
}