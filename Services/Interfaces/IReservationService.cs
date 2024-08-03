using CaffeBar.Models;

namespace CaffeBar.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetReservationsAsync();
        Task<Reservation?> GetReservationAsync(int id);
        Task<bool> CreateReservationAsync(Reservation newReservation);
        Task<bool> UpdateReservationAsync(Reservation updatedReservation);
        Task<bool> DeleteReservationAsync(int id);
    }
}