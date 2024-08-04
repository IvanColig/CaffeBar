using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CaffeBar.Models;
using CaffeBar.Services;

namespace CaffeBar.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // GET: MenuItem
        public async Task<IActionResult> Index()
        {
            var menuItems = await _menuItemService.GetMenuItems();
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
