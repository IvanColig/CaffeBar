using CaffeBar.Data;
using CaffeBar.Models;

namespace CaffeBar.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly CaffeBarDbContext _context;

        public MenuItemService(CaffeBarDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetMenuItems()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItem(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<bool> CreateMenuItem(MenuItem menuItem)
        {
            if (!IsValidMenuItem(menuItem))
            {
                return false;
            }

            try
            {
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMenuItem(MenuItem menuItem)
        {
            if (!IsValidMenuItem(menuItem))
            {
                return false;
            }

            try
            {
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteMenuItem(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return false;
            }

            try
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValidMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(menuItem.Name))
            {
                return false;
            }

            if (menuItem.Price <= 0)
            {
                return false;
            }

            return true;
        }
    }
}