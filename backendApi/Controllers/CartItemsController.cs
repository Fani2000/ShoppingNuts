using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartItemsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CartItem>> GetAllItems()
        {
            return _cartService.GetAllItems();
        }

        [HttpGet("{id}")]
        public ActionResult<CartItem> GetItem(int id)
        {
            var item = _cartService.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<CartItem> CreateItem(CartItem item)
        {
            var createdItem = _cartService.AddItem(item);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public ActionResult<CartItem> UpdateItem(int id, CartItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var updatedItem = _cartService.UpdateItem(item);
            if (updatedItem == null)
            {
                return NotFound();
            }

            return updatedItem;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(int id)
        {
            var success = _cartService.DeleteItem(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}