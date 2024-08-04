using CaffeBar.Data;
using CaffeBar.Models;
using Microsoft.AspNetCore.Hosting;

namespace CaffeBar.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuItemService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                if (menuItem.Image != null)
                {
                    string imagePath = await SaveImageAsync(menuItem.Image);
                    menuItem.ImagePath = imagePath;
                }

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
                if (menuItem.Image != null)
                {
                    if (!string.IsNullOrEmpty(menuItem.ImagePath))
                    {
                        DeleteImage(menuItem.ImagePath);
                    }

                    string imagePath = await SaveImageAsync(menuItem.Image);
                    menuItem.ImagePath = imagePath;
                }

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
                if (!string.IsNullOrEmpty(menuItem.ImagePath))
                {
                    DeleteImage(menuItem.ImagePath);
                }

                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        private void DeleteImage(string imagePath)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
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