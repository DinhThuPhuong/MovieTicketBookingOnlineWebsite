using DoAnCoSoTL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        // GET: Seat/Choose
        public IActionResult Choose(int screeningId, string timeSlot)
        {
            var screening = _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null)
            {
                return NotFound();
            }

            ViewData["ScreeningId"] = screeningId;
            ViewData["TimeSlot"] = timeSlot;
            ViewBag.TicketPrice = screening.Movie.Price;

            var bookedSeatIds = _context.Seats
                .Where(s => s.ScreeningId == screeningId && s.IsBooked)
                .Select(s => s.Id)
                .ToList();

            return View(bookedSeatIds);
        }


        //// POST: Seat/BookTicket
        //[HttpPost]
        //public async Task<IActionResult> BookTicket(int screeningId, string timeSlot, List<int> selectedSeatIds)
        //{
        //    var screening = await _context.Screenings
        //        .Include(s => s.Movie)
        //        .FirstOrDefaultAsync(s => s.Id == screeningId);

        //    if (screening == null)
        //    {
        //        return NotFound();
        //    }

        //    double ticketPrice = screening.Movie.Price;
        //    double totalPrice = ticketPrice * selectedSeatIds.Count;

        //    return RedirectToAction("TicketDetails", new { screeningId = screeningId, timeSlot = timeSlot, selectedSeatIds = selectedSeatIds, totalPrice = totalPrice });
        //}
        // POST: Seat/BookTicket
        [HttpPost]
        public async Task<IActionResult> BookTicket(int screeningId, string timeSlot, List<string> selectedSeatIds)
        {
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == screeningId);

            if (screening == null)
            {
                return NotFound();
            }

            double ticketPrice = screening.Movie.Price;
            double totalPrice = ticketPrice * selectedSeatIds.Count;

            // Truyền dữ liệu qua ViewBag để sử dụng trong TicketDetails
            ViewBag.SelectedSeats = selectedSeatIds;
            ViewBag.TotalPrice = totalPrice;

            //return RedirectToAction("TicketDetails", new { screeningId = screeningId, timeSlot = timeSlot });
            return RedirectToAction("TicketDetails", new { screeningId = screeningId, timeSlot = timeSlot, selectedSeatIds = selectedSeatIds, totalPrice = totalPrice });
        }



        //// GET: Seat/TicketDetails
        //public IActionResult TicketDetails(int screeningId, string timeSlot, string selectedSeatIds, double totalPrice)
        //{
        //    var screening = _context.Screenings
        //        .Include(s => s.Movie)
        //        .Include(s => s.Cinema)
        //        .FirstOrDefault(s => s.Id == screeningId);

        //    if (screening == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.MovieName = screening.Movie.Name;
        //    ViewBag.TimeSlot = timeSlot;
        //    ViewBag.CinemaName = screening.Cinema.Name;
        //    ViewBag.CinemaLocation = screening.Cinema.Location;
        //    //ViewBag.SelectedSeats = selectedSeatIds;
        //    //ViewBag.TotalPrice = totalPrice;
        //    ViewBag.SelectedSeatIds = selectedSeatIds.Split(',');
        //    ViewBag.TotalPrice = totalPrice;

        //    return View();
        //}
        // GET: Seat/TicketDetails
        public IActionResult TicketDetails(int screeningId, string timeSlot, string selectedSeatIds, double totalPrice)
        {
            var screening = _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null)
            {
                return NotFound();
            }

            ViewBag.MovieName = screening.Movie.Name;
            ViewBag.TimeSlot = timeSlot;
            ViewBag.CinemaName = screening.Cinema.Name;
            ViewBag.CinemaLocation = screening.Cinema.Location;

            // Kiểm tra nếu selectedSeatIds không phải null trước khi sử dụng Split(',')
            ViewBag.SelectedSeatIds = selectedSeatIds != null ? selectedSeatIds.Split(',') : new string[0];

            ViewBag.TotalPrice = totalPrice;

            return View();
        }


    }
}
