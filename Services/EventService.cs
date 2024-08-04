using CaffeBar.Data;
using CaffeBar.Models;
using Microsoft.AspNetCore.Hosting;

namespace Caffebar.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event?> GetEventAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<bool> CreateEventAsync(Event newEvent)
        {
            if (!IsValidEvent(newEvent))
            {
                return false;
            }

            try
            {
                if (newEvent.Image != null)
                {
                    string imagePath = await SaveImageAsync(newEvent.Image);
                    newEvent.ImagePath = imagePath;
                }

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEventAsync(Event updatedEvent)
        {
            if (!IsValidEvent(updatedEvent))
            {
                return false;
            }

            try
            {
                if (updatedEvent.Image != null)
                {
                    if (!string.IsNullOrEmpty(updatedEvent.ImagePath))
                    {
                        DeleteImage(updatedEvent.ImagePath);
                    }

                    string imagePath = await SaveImageAsync(updatedEvent.Image);
                    updatedEvent.ImagePath = imagePath;
                }

                _context.Events.Update(updatedEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(@event.ImagePath))
                {
                    DeleteImage(@event.ImagePath);
                }

                _context.Events.Remove(@event);
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

        private bool IsValidEvent(Event @event)
        {
            if (@event == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(@event.Title))
            {
                return false;
            }

            return true;
        }
    }
}
