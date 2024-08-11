using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CaffeBar.Data;
using CaffeBar.Models;
using CaffeBar.Services;
using Microsoft.AspNetCore.Identity;

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
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetReservationsAsync();
            return View(reservations);
        }

        // GET: Reservation/Details/5
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

            ViewData["User"] = await _userManager.GetUserAsync(User);;
            ViewData["TableOptions"] = new SelectList(await _reservationService.GetTableOptionsAsync(), "Value", "Text");
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableId,Date,Time")] Reservation reservation)
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
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Unable to create reservation. Please try again.");
            }

            ViewData["User"] = await _userManager.GetUserAsync(User);
            ViewData["TableOptions"] = new SelectList(await _reservationService.GetTableOptionsAsync(), "Value", "Text");
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

            ViewData["User"] = await _userManager.GetUserAsync(User);;
            ViewData["TableOptions"] = new SelectList(await _reservationService.GetTableOptionsAsync(), "Value", "Text");

            return View(reservation);
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TableId,Date,Time")] Reservation reservation)
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
            ViewData["TableOptions"] = new SelectList(await _reservationService.GetTableOptionsAsync(), "Value", "Text");
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
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
