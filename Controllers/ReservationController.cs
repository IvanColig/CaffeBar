using CaffeBar.Data;
using CaffeBar.Models;
using CaffeBar.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaffeBar.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(IReservationService reservationService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        private async Task<string?> GetUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        // GET: Reservation
        [Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetReservationsAsync();
            return View(reservations);
        }

        // GET: MyReservations
        
        public async Task<IActionResult> MyReservations()
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            var reservations = await _reservationService.MyReservationsAsync(userId);
            return View(reservations);
        }

        // GET: Reservation/Details/5
        [Authorize(Roles="Admin,Waiter")]
        public async Task<IActionResult> Details(int? id)
        {
            var userId = await GetUserIdAsync();

            if (userId == null || id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationAsync(id.Value, userId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public async Task<IActionResult> Create()
        {
            var user = await GetUserIdAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            ViewData["User"] = await _userManager.GetUserAsync(User);
            ViewData["TableOptions"] = new List<SelectListItem>(); // Initial empty list
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Time,TableId")] Reservation reservation)
        {
            var userId = await GetUserIdAsync();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                reservation.IdentityUserId = userId;
                var result = await _reservationService.CreateReservationAsync(reservation, userId);
                if (result)
                {
                    return RedirectToAction(nameof(MyReservations));
                }
                ModelState.AddModelError(string.Empty, "Unable to create reservation. Please try again.");
            }

            ViewData["User"] = await _userManager.GetUserAsync(User);
            ViewData["TableOptions"] = await PopulateTableOptions(); // Repopulate with actual table options in case of error
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = await GetUserIdAsync();

            if (userId == null || id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationAsync(id.Value, userId);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.TableOptions = await _reservationService.GetTableOptionsAsync(reservation.Date, reservation.Time);
            ViewData["User"] = await _userManager.GetUserAsync(User);
            return View(reservation);
        }


        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,TableId")] Reservation reservation)
        {
            var userId = await GetUserIdAsync();

            if (userId == null || id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                reservation.IdentityUserId = userId;
                var result = await _reservationService.UpdateReservationAsync(reservation, userId);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Unable to update reservation. Please try again.");
            }

            ViewData["User"] = await _userManager.GetUserAsync(User);
            ViewBag.TableOptions = await PopulateTableOptions(reservation.TableId); // Repopulate with actual table options in case of error
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = await GetUserIdAsync();

            if (userId == null || id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationAsync(id.Value, userId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = await GetUserIdAsync();

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _reservationService.DeleteReservationAsync(id, userId);
            if (result)
            {
                return RedirectToAction(nameof(MyReservations));
            }

            return NotFound();
        }

        // GET: Available Tables for Date and Time
        [HttpGet]
        public async Task<IActionResult> GetAvailableTables(DateTime date, TimeSpan time)
        {
            var tableOptions = await _reservationService.GetTableOptionsAsync(date, time);
            return Json(tableOptions);
        }

        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _reservationService.GetAllTablesAsync();
            return Json(tables);
        }

        public async Task<SelectList> PopulateTableOptions(int? selectedTableId = null)
        {
            var tables = await _reservationService.GetAllTablesAsync();
            
            if (tables == null || !tables.Any())
            {
                tables = new List<Table>
                {
                    new Table { Id = 0, Seats = 0, IsReserved = false }
                };
            }

            return new SelectList(tables, "Id", "Id", selectedTableId);
        }

    }
}
