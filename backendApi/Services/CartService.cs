using backend.Models;
using backendApi.Utils;

namespace backend.Services
{
    public class CartService : ICartService
    {
        public CartService()
        {
            // Constructor is now empty as we're using the static CartUtils
        }

        public List<CartItem> GetAllItems()
        {
            return CartUtils.GetCartItems();
        }

        public CartItem GetItemById(int id)
        {
            return CartUtils.GetCartItems().FirstOrDefault(i => i.Id == id);
        }

        public CartItem AddItem(CartItem item)
        {
            // Only generate ID if the item is new (doesn't exist in list yet)
            var existingItem = GetItemById(item.Id);
            if (existingItem == null && item.Id == 0)
            {
                item.Id = GetNextId();
            }

            // If item with this ID already exists, update it instead of adding
            if (existingItem != null)
            {
                return UpdateItem(item);
            }

            return CartUtils.AddCartItem(item);
        }

        public CartItem UpdateItem(CartItem item)
        {
            var existingItem = GetItemById(item.Id);
            if (existingItem == null)
            {
                return null;
            }

            existingItem.Name = item.Name;
            existingItem.Category = item.Category;
            existingItem.Description = item.Description;
            existingItem.Size = item.Size;
            existingItem.Quantity = item.Quantity;
            existingItem.Price = item.Price;
            existingItem.Image = item.Image;

            return existingItem;
        }

        public bool DeleteItem(int id)
        {
            return CartUtils.RemoveCartItem(id);
        }

        public int GetNextId()
        {
            return CartUtils.GetNextId();
        }
    }
}