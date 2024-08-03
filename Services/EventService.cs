using CaffeBar.Data;
using CaffeBar.Models;

namespace Caffebar.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
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
            try
            {
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
            try
            {
                _context.Events.Update(updatedEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public async Task<bool> DeleteEventAsync(int id)
        {   
            if(id <= 0)
            {
                return false;
            }
            
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return false;
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}