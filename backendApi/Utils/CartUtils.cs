using backend.Models;

namespace backendApi.Utils
{
    public static class CartUtils
    {
        // Static list to store cart items across the application
        private static readonly List<CartItem> _cartItems = new List<CartItem>();

        // Method to get all cart items
        public static List<CartItem> GetCartItems()
        {
             _cartItems.Add(new CartItem
            {
                Id = 1,
                Name = "Mixed Nuts (Almond, Brazil, Cashew, Macadamia)",
                Category = "Standard Nuts",
                Description =
                    "A premium mix of four powerhouse nuts offering creamy, crunchy, and earthy flavors in one nutritious bite.",
                Size = "500g",
                Quantity = 1,
                Price = 135.00m,
                Image =
                    "https://cdn.shopify.com/s/files/1/0081/6968/5089/products/mixed-tree-nuts-exotic-raw-1kg-526597_540x.jpg?v=1692805255"
            });

            _cartItems.Add(new CartItem
            {
                Id = 2,
                Name = "Cashews (Raw, Roasted and Salted, Peri-Peri)",
                Category = "Standard Nuts",
                Description =
                    "Buttery, smooth cashews in a variety of natural, roasted, or spicy peri-peri flavors. High in magnesium and healthy fats.",
                Size = "500g",
                Quantity = 1,
                Price = 160.00m,
                Image = "https://goosebumps.store/cdn/shop/files/Peri-Peri-Cashews-6_1800x1800.jpg?v=1731749434"
            });

            _cartItems.Add(new CartItem
            {
                Id = 3,
                Name = "Almonds (Roasted and Salted)",
                Category = "Standard Nuts",
                Description =
                    "Crunchy, protein-rich almonds roasted to perfection and lightly salted to enhance their natural flavor.",
                Size = "500g",
                Quantity = 1,
                Price = 130.00m,
                Image =
                    "https://tse1.explicit.bing.net/th/id/OIP.8CE0QlFao4dO9WuhSxIaVAHaHa?w=853&h=853&rs=1&pid=ImgDetMain&o=7&rm=3"
            });

            _cartItems.Add(new CartItem
            {
                Id = 4,
                Name = "Pistachios in Shell (Roasted and Salted)",
                Category = "Standard Nuts",
                Description =
                    "Delicious in-shell pistachios roasted and salted, perfect for snacking and heart health.",
                Size = "500g",
                Quantity = 1,
                Price = 200.00m,
                Image = "https://tse4.mm.bing.net/th/id/OIP.FjSfL3rnVHrt_pSe_LXAgwHaHa?rs=1&pid=ImgDetMain&o=7&rm=3"
            });

            _cartItems.Add(new CartItem
            {
                Id = 5,
                Name = "Walnuts Halves",
                Category = "Standard Nuts",
                Description = "Crunchy, mildly bitter walnuts packed with omega-3s for brain and heart health.",
                Size = "500g",
                Quantity = 1,
                Price = 150.00m,
                Image = "https://www.shugarysweets.com/wp-content/uploads/2022/03/toasted-pecans-recipe.jpg"
            });

            _cartItems.Add(new CartItem
            {
                Id = 6,
                Name = "Macadamia (Raw, Roasted and Salted)",
                Category = "Standard Nuts",
                Description =
                    "Rich, buttery macadamias that are creamy and crunchy—perfect for indulgent healthy snacking.",
                Size = "500g",
                Quantity = 1,
                Price = 130.00m,
                Image = "https://nutsandall.co.za/wp-content/uploads/2020/04/NutsAll__47.jpg"
            });

            _cartItems.Add(new CartItem
            {
                Id = 7,
                Name = "Brazil (Raw)",
                Category = "Standard Nuts",
                Description = "Selenium-packed Brazil nuts with a crunchy texture and a slightly earthy flavor.",
                Size = "500g",
                Quantity = 1,
                Price = 230.00m,
                Image = "https://tse2.mm.bing.net/th/id/OIP.GSd0ed7-pTLhqsCBMsFxLgHaHa?rs=1&pid=ImgDetMain&o=7&rm=3"
            });

            _cartItems.Add(new CartItem
            {
                Id = 8,
                Name = "Pine Kernels",
                Category = "Standard Nuts",
                Description = "Delicate, buttery pine nuts perfect for salads, pesto, and boosting metabolism.",
                Size = "250g",
                Quantity = 1,
                Price = 180.00m,
                Image = "https://bostonandco.co.za/wp-content/uploads/2020/10/pine-nuts.jpg"
            });

            _cartItems.Add(new CartItem
            {
                Id = 9,
                Name = "Almonds Blenched",
                Category = "Standard Nuts",
                Description = "Blanched almonds without skins—smooth, soft texture, perfect for baking and snacking.",
                Size = "500g",
                Quantity = 1,
                Price = 150.00m,
                Image =
                    "https://i2.wp.com/zimmermansonline.com/wp-content/uploads/2020/04/IMG_5500-scaled.jpg?resize=1152%2C1536&ssl=1"
            });
            
            return _cartItems;
        }

        // Add a new cart item
        public static CartItem AddCartItem(CartItem item)
        {
            _cartItems.Add(item);
            return item;
        }

        // Remove a cart item
        public static bool RemoveCartItem(int id)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return false;
            }

            return _cartItems.Remove(item);
        }

        // Clear all cart items
        public static void ClearCart()
        {
            _cartItems.Clear();
        }

        // Get the next available ID
        public static int GetNextId()
        {
            return _cartItems.Count == 0 ? 1 : _cartItems.Max(i => i.Id) + 1;
        }
    }
}