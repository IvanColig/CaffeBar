using CaffeBar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CaffeBar.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order?> GetOrderAsync(int id, string userId);
        Task<bool> CreateOrderAsync(Order newOrder, string userId);
        Task<bool> UpdateOrderAsync(Order updatedOrder, string userId);
        Task<bool> DeleteOrderAsync(int id, string userId);
        Task<IEnumerable<SelectListItem>> GetTableOptionsAsync();
        Task<IEnumerable<SelectListItem>> GetMenuItemOptionsAsync();
    }
}
