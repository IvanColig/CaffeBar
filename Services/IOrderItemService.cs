using CaffeBar.Models;

namespace CaffeBar.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync();
        Task<OrderItem?> GetOrderItemAsync(int id);
        Task<bool> CreateOrderItemAsync(OrderItem newOrderItem);
        Task<bool> UpdateOrderItemAsync(OrderItem updatedOrderItem);
        Task<bool> DeleteOrderItemAsync(int id);
    }
}
