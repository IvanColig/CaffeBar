using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CaffeBar.Models;
using CaffeBar.Services;

namespace CaffeBar.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly UserManager<ApplicationUser> _usermanager;

        public MenuItemController(IMenuItemService menuItemService, UserManager<ApplicationUser> usermanager)
        {
            _menuItemService = menuItemService;
            _usermanager = usermanager;
        }

        // GET: MenuItem
        public async Task<IActionResult> Index()
        {
            var user = await _usermanager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            var menuItems = await _menuItemService.GetMenuItems();
            
            ViewBag.IsAdmin = isAdmin;
            return View(menuItems);
        }

        // GET: MenuItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuItemService.GetMenuItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItem/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] MenuItem menuItem, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                menuItem.Image = Image;
                bool isCreated = await _menuItemService.CreateMenuItem(menuItem);
                if (isCreated)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to create menu item.");
            }
            return View(menuItem);
        }

        // GET: MenuItem/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuItemService.GetMenuItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImagePath")] MenuItem menuItem, IFormFile? Image)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                menuItem.Image = Image;
                bool isUpdated = await _menuItemService.UpdateMenuItem(menuItem);
                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to update menu item.");
            }
            return View(menuItem);
        }

        // GET: MenuItem/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuItemService.GetMenuItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await _menuItemService.DeleteMenuItem(id);
            if (isDeleted)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Unable to delete menu item.");
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
