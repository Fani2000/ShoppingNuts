
namespace backend.Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderById(string id);
        Order CreateOrder(Order order);
        Order UpdateOrder(Order order);
        bool DeleteOrder(string id);
        string GenerateOrderId();
    }
}