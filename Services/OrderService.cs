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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _userId;

        public OrderService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task EnsureUserIdAsync()
{
    if (_userId == null)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext != null && httpContext.User != null)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            _userId = user?.Id;
        }
    }
}


        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            await EnsureUserIdAsync();
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.IdentityUserId == _userId);
        }

        public async Task<bool> CreateOrderAsync(Order newOrder)
        {
            if (newOrder == null ||
                newOrder.OrderItems == null ||
                newOrder.OrderItems.Count == 0 ||
                newOrder.TableId <= 0)
            {
                return false;
            }

            try
            {
                await EnsureUserIdAsync();
                if (_userId == null) return false;

                newOrder.IdentityUserId = _userId;
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(Order updatedOrder)
        {
            if (updatedOrder == null ||
                updatedOrder.Id <= 0 ||
                updatedOrder.OrderItems == null ||
                updatedOrder.OrderItems.Count == 0 ||
                updatedOrder.TableId <= 0)
            {
                return false;
            }

            await EnsureUserIdAsync();
            if (updatedOrder.IdentityUserId != _userId)
            {
                return false;
            }

            try
            {
                _context.Orders.Update(updatedOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            await EnsureUserIdAsync();
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.IdentityUserId == _userId);

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
    }
}
