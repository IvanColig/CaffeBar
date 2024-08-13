using CaffeBar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CaffeBar.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetReservationsAsync();
        Task<Reservation?> GetReservationAsync(int id, string userId);
        Task<IEnumerable<Reservation>> MyReservationsAsync(string userId);
        Task<bool> CreateReservationAsync(Reservation newReservation, string userId);
        Task<bool> UpdateReservationAsync(Reservation updatedReservation, string userId);
        Task<bool> DeleteReservationAsync(int id, string userId);
        Task<IEnumerable<SelectListItem>> GetTableOptionsAsync(DateTime date, TimeSpan time);
        Task<IEnumerable<Table>> GetAllTablesAsync();
    }
}