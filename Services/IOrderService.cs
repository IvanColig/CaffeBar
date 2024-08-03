using CaffeBar.Models;

namespace CaffeBar.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order?> GetOrderAsync(int id);
        Task<bool> CreateOrderAsync(Order newOrder);
        Task<bool> UpdateOrderAsync(Order updatedOrder);
        Task<bool> DeleteOrderAsync(int id);
    }
}
