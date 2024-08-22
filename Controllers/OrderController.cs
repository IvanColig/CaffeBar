using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CaffeBar.Services;
using CaffeBar.Models;

namespace CaffeBar.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user;
        }


        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = (await GetUserAsync())?.Id;

            if (userId == null || id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderAsync(id.Value, userId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            ViewData["User"] = await GetUserAsync();
            ViewData["TableOptions"] = new SelectList(await _orderService.GetTableOptionsAsync(), "Value", "Text");
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Order order)
        {
            var userId = (await GetUserAsync())?.Id;

            if (userId == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newOrder = await _orderService.CreateOrderAsync(userId);
                if (newOrder != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Order creation failed.");
            }

            ViewData["User"] = await GetUserAsync();
            ViewData["TableOptions"] = new SelectList(await _orderService.GetTableOptionsAsync(), "Value", "Text");
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = (await GetUserAsync())?.Id;

            if (userId == null || id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderAsync(id.Value, userId);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["User"] = await GetUserAsync();
            ViewData["TableOptions"] = new SelectList(await _orderService.GetTableOptionsAsync(), "Value", "Text");
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,TableId")] Order order)
        {
            var userId = (await GetUserAsync())?.Id;

            if (id != order.Id || userId == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateOrderAsync(order, userId);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Order update failed.");
            }

            ViewData["User"] = await GetUserAsync();
            ViewData["TableOptions"] = new SelectList(await _orderService.GetTableOptionsAsync(), "Value", "Text");
            return View(order);
        }

        public async Task<IActionResult> AddToOrder(int orderId, int menuItemId)
        {
            var userId = (await GetUserAsync())?.Id;

            if (userId == null)
            {
                return NotFound();
            }

            await _orderService.AddToOrderAsync(orderId, userId, menuItemId);
            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = (await GetUserAsync())?.Id;

            if (id == null || userId == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderAsync(id.Value, userId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = (await GetUserAsync())?.Id;

            if (userId == null)
            {
                return NotFound();
            }

            var result = await _orderService.DeleteOrderAsync(id, userId);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
