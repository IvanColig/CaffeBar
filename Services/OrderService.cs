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


        public async Task<Order> CreateOrderAsync(string userId)
        {
            var order = new Order
            {
                IdentityUserId = userId,
                TableId = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task AddToOrderAsync(int orderId, string userId, int menuItemId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.IdentityUserId == userId);

            if (order == null)
            {
                return;
            }

            var menuItem = await _context.MenuItems.FindAsync(menuItemId);

            if (menuItem == null)
            {
                return;
            }

            if(order.OrderItems.Any(oi => oi.MenuItemId == menuItemId))
            {
                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.MenuItemId == menuItemId);
                if (orderItem != null)
                {
                    orderItem.Quantity++;
                }
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = orderId,
                    MenuItemId = menuItemId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
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
