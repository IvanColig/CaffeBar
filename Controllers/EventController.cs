using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CaffeBar.Models;
using Caffebar.Services;

namespace CaffeBar.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetEventsAsync();
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _eventService.GetEventAsync(id.Value);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Event/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,Date")] Event eventItem, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                // Assign image file to event
                eventItem.Image = Image;
                bool isCreated = await _eventService.CreateEventAsync(eventItem);
                if (isCreated)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to create event.");
            }
            return View(eventItem);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _eventService.GetEventAsync(id.Value);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,ImagePath")] Event eventItem, IFormFile? Image)
        {
            if (id != eventItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Create a new instance of Event for update with the image
                var updatedEvent = await _eventService.GetEventAsync(id);
                if (updatedEvent == null)
                {
                    return NotFound();
                }

                // Update properties
                updatedEvent.Title = eventItem.Title;
                updatedEvent.Description = eventItem.Description;
                updatedEvent.Date = eventItem.Date;

                // Handle image update
                if (Image != null)
                {
                    updatedEvent.Image = Image;
                }

                bool isUpdated = await _eventService.UpdateEventAsync(updatedEvent);
                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to update event.");
            }
            return View(eventItem);
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _eventService.GetEventAsync(id.Value);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await _eventService.DeleteEventAsync(id);
            if (isDeleted)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Unable to delete event.");
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
