using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CaffeBar.Data;
using CaffeBar.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CaffeBar.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int id, string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.IdentityUserId == userId);
        }

        public async Task<bool> CreateOrderAsync(Order newOrder, string userId)
        {
            newOrder.IdentityUserId = userId;

            if (newOrder == null ||
                newOrder.OrderItems == null ||
                newOrder.OrderItems.Count == 0 ||
                newOrder.TableId <= 0 ||
                newOrder.IdentityUserId == null)
            {
                return false;
            }

            try
            {
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(Order updatedOrder, string userId)
        {
            if (updatedOrder == null ||
                updatedOrder.Id <= 0 ||
                updatedOrder.OrderItems == null ||
                updatedOrder.OrderItems.Count == 0 ||
                updatedOrder.TableId <= 0)
            {
                return false;
            }

            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id && o.IdentityUserId == userId);

            if (existingOrder == null)
            {
                return false;
            }

             try
            {
                _context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int id, string userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.IdentityUserId == userId);

            if (order == null)
            {
                return false;
            }

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetTableOptionsAsync()
        {
            return await _context.Tables
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Id.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetMenuItemOptionsAsync()
        {
            return await _context.MenuItems
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
                .ToListAsync();
        }
    }
}
