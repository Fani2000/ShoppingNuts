using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public interface ICartService
    {
        List<CartItem> GetAllItems();
        CartItem GetItemById(int id);
        CartItem AddItem(CartItem item);
        CartItem UpdateItem(CartItem item);
        bool DeleteItem(int id);
        int GetNextId();
    }
}