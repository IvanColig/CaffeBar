using CaffeBar.Data;
using CaffeBar.Models;

namespace CaffeBar.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly CaffeBarDbContext _context;

        public OrderItemService(CaffeBarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.MenuItem)
                .ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemAsync(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.MenuItem)
                .FirstOrDefaultAsync(oi => oi.OrderId == id);
        }

        public async Task<bool> CreateOrderItemAsync(OrderItem newOrderItem)
        {
            if (newOrderItem == null)
            {
                return false;
            }

            try
            {
                _context.OrderItems.Add(newOrderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderItemAsync(OrderItem updatedOrderItem)
        {
            if (updatedOrderItem == null || updatedOrderItem.OrderId <= 0 || updatedOrderItem.MenuItemId <= 0)
            {
                return false;
            }

            try
            {
                _context.OrderItems.Update(updatedOrderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            
            if (orderItem == null)
            {
                return false;
            }

            try
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
