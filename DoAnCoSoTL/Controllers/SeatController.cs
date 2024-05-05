using DoAnCoSoTL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnCoSoTL.Controllers
{
    public class SeatController : Controller
    {
        private readonly MovieContext _context;

        public SeatController(MovieContext context)
        {
            _context = context;
        }

        // GET: Seat/ChooseSeat?screeningId=5
        public async Task<IActionResult> ChooseSeat(int? screeningId)
        {
            if (screeningId == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(screeningId);

            if (screening == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas.FindAsync(screening.CinemaId);

            if (cinema == null)
            {
                return NotFound();
            }

            var seats = await _context.Seats
                .Where(s => s.ScreeningId == screeningId)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToListAsync();

            ViewBag.ScreeningId = screeningId; // Đặt giá trị ScreeningId vào ViewBag

            ViewData["ScreeningId"] = screeningId;
            ViewData["CinemaName"] = cinema.Name;

            return View(seats);
        }


        // POST: Seat/ChooseSeat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseSeat(int? screeningId, int[] selectedSeats)
        {
            if (selectedSeats == null || selectedSeats.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one seat.");
                return RedirectToAction(nameof(ChooseSeat), new { screeningId });
            }

            foreach (var seatId in selectedSeats)
            {
                var seat = await _context.Seats.FindAsync(seatId);
                if (seat != null)
                {
                    seat.IsAvailable = false;
                    _context.Update(seat);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
