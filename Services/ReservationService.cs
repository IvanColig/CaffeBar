using CaffeBar.Data;
using CaffeBar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaffeBar.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation?> GetReservationAsync(int id, string userId)
        {
            return await _context.Reservations
                .Where(r => r.Id == id && r.IdentityUserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateReservationAsync(Reservation newReservation, string userId)
        {
            if (newReservation == null)
            {
                throw new ArgumentNullException(nameof(newReservation), "Reservation cannot be null.");
            }

            if (newReservation.TableId <= 0 || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Invalid table ID or user ID.");
            }

            var isTableReserved = await _context.Reservations
                .AnyAsync(r => r.TableId == newReservation.TableId && r.Date == newReservation.Date && r.Time == newReservation.Time);

            if (isTableReserved)
            {
                throw new InvalidOperationException("The table is already reserved for the selected date and time.");
            }

            try
            {
                newReservation.IdentityUserId = userId;
                _context.Reservations.Add(newReservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> UpdateReservationAsync(Reservation updatedReservation, string userId)
        {
            if (updatedReservation == null)
            {
                throw new ArgumentNullException(nameof(updatedReservation), "Reservation cannot be null.");
            }

            if (updatedReservation.Id <= 0 || updatedReservation.TableId <= 0 || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Invalid reservation ID, table ID, or user ID.");
            }

            var existingReservation = await _context.Reservations
                .Where(r => r.Id == updatedReservation.Id && r.IdentityUserId == userId)
                .FirstOrDefaultAsync();

            if (existingReservation == null)
            {
                return false;
            }

            try
            {
                existingReservation.TableId = updatedReservation.TableId;
                existingReservation.Date = updatedReservation.Date;
                existingReservation.Time = updatedReservation.Time;

                _context.Reservations.Update(existingReservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> DeleteReservationAsync(int id, string userId)
        {
            if (id <= 0 || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Invalid reservation ID or user ID.");
            }

            var reservation = await _context.Reservations
                .Where(r => r.Id == id && r.IdentityUserId == userId)
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return false;
            }

            try
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetTableOptionsAsync()
        {
            var reservedTableIds = await _context.Reservations
                .Select(r => r.TableId)
                .Distinct()
                .ToListAsync();
            
            return await _context.Tables
                .Where(t => !reservedTableIds.Contains(t.Id))
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"Table {t.Id}" 
                })
                .ToListAsync();
        }
    }
}
