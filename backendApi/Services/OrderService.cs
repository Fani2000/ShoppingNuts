using backend.Models;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();

        public OrderService()
        {
            // Constructor initialization if needed
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        public Order GetOrderById(string id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public Order CreateOrder(Order order)
        {
            // Only generate ID if not provided
            if (string.IsNullOrEmpty(order.Id))
            {
                order.Id = GenerateOrderId();
            }

            // Check if order with this ID already exists - if so, update instead
            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                return UpdateOrder(order);
            }

            _orders.Add(order);
            return order;
        }

        public Order UpdateOrder(Order order)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder == null)
            {
                return null;
            }

            // Update properties of existing order
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.Items = order.Items;
            existingOrder.Total = order.Total;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Status = order.Status;
            // Update other properties as needed

            return existingOrder;
        }

        public bool DeleteOrder(string id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return false;
            }

            _orders.Remove(order);
            return true;
        }

        public string GenerateOrderId()
        {
            // Generate a unique order ID
            return "ORD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}