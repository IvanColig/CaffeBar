using CaffeBar.Models;

namespace CaffeBar.Services
{
    public interface IMenuItemService
    {
        Task<List<MenuItem>> GetMenuItems();
        Task<MenuItem?> GetMenuItem(int id);
        Task<bool> CreateMenuItem(MenuItem menuItem);
        Task<bool> UpdateMenuItem(MenuItem menuItem);
        Task<bool> DeleteMenuItem(int id);
    }
}