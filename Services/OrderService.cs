using CaffeBar.Data;
using CaffeBar.Models;

namespace CaffeBar.Services
{
    public class OrderService : IOrderService
    {
        private readonly CaffeBarDbContext _context;

        public OrderService(CaffeBarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> CreateOrderAsync(Order newOrder)
        {
            if (newOrder == null ||
                newOrder.Id > 0 ||
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

        public async Task<bool> UpdateOrderAsync(Order updatedOrder)
        {
            if (updatedOrder == null ||
                updatedOrder.Id <= 0 ||
                updatedOrder.OrderItems == null ||
                updatedOrder.OrderItems.Count == 0 ||
                updatedOrder.TableId <= 0 ||
                updatedOrder.IdentityUserId == null)
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
            var order = await _context.Orders.FindAsync(id);

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
    }
}
