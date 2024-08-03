using CaffeBar.Data;
using CaffeBar.Models;

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

        public async Task<Reservation?> GetReservationAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<bool> CreateReservationAsync(Reservation newReservation)
        {
            if (newReservation == null)
            {
                throw new ArgumentNullException("Reservation cannot be null.");
            }

            if (newReservation.TableId <= 0 || newReservation.IdentityUserId == null)
            {
                throw new ArgumentException("Invalid table ID or user ID.");
            }

            try
            {
                _context.Reservations.Add(newReservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateReservationAsync(Reservation updatedReservation)
        {
            if (updatedReservation == null)
            {
                throw new ArgumentNullException("Reservation cannot be null.");
            }

            if (updatedReservation.Id <= 0 || updatedReservation.TableId <= 0 || updatedReservation.IdentityUserId == null)
            {
                throw new ArgumentException("Invalid reservation ID, table ID, or user ID.");
            }

            try
            {
                _context.Reservations.Update(updatedReservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid reservation ID.");
            }

            var reservation = await _context.Reservations.FindAsync(id);
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
    }
}