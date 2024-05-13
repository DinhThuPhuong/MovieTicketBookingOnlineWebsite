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
        //public IActionResult Choose(int screeningId, string timeSlot)
        //{
        //    var screening = _context.Screenings
        //        .Include(s => s.Movie)
        //        .FirstOrDefault(s => s.Id == screeningId);

        //    if (screening == null)
        //    {
        //        return NotFound();
        //    }
        //    var seats = GenerateSeatsForScreening(screeningId);
        //    ViewData["ScreeningId"] = screeningId;
        //    ViewData["TimeSlot"] = timeSlot;
        //    ViewBag.TicketPrice = screening.Movie.Price;

        //    //var bookedSeatIds = _context.Seats
        //    //    .Where(s => s.ScreeningId == screeningId && s.IsBooked)
        //    //    .Select(s => s.Id)
        //    //    .ToList();

        //    //return View(bookedSeatIds);
        //    return View();
        //}
        public IActionResult Choose(int screeningId, string timeSlot)
        {
            var seats = GenerateSeatsForScreening(screeningId);

            ViewData["ScreeningId"] = screeningId;
            ViewData["TimeSlot"] = timeSlot;
            ViewBag.TicketPrice = _context.Screenings.Include(s => s.Movie).FirstOrDefault(s => s.Id == screeningId)?.Movie?.Price ?? 0;

            return View(seats); // Pass the seats list to the view
        }

        //    public IActionResult Choose(int screeningId, string timeSlot)
        //    {
        //        // Kiểm tra xem khung giờ chiếu và rạp có tồn tại trong cơ sở dữ liệu không
        //        //var screening = _context.Screenings
        //        //    .Include(s => s.Cinema)
        //        //    .FirstOrDefault(s => s.Id == screeningId && s.TimeSlot == timeSlot);
        //        var screening = _context.Screenings
        //.Include(s => s.Cinema)
        //.ToList() // Evaluate part of the query on the client side
        //.FirstOrDefault(s => s.Id == screeningId && s.TimeSlot == timeSlot);

        //        if (screening == null)
        //        {
        //            return NotFound();
        //        }

        //        // Tạo danh sách các ghế cho khung giờ và rạp đã chọn
        //        var seats = GenerateSeatsForScreening(screeningId);

        //        // Lưu danh sách các ghế vào ViewBag hoặc ViewData để truyền sang view
        //        ViewData["ScreeningId"] = screeningId;
        //        ViewData["TimeSlot"] = timeSlot;
        //        ViewBag.Seats = seats;

        //        // Trả về trang Choose với danh sách các ghế
        //        return View();
        //    }

        // Phương thức để tạo danh sách các ghế cho một khung giờ chiếu và lưu chúng vào cơ sở dữ liệu
        private List<Seat> GenerateSeatsForScreening(int screeningId)
        {
            // Lấy thông tin về khung giờ chiếu để tạo các ghế
            var screening = _context.Screenings
                .Include(s => s.Cinema)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null)
            {
                return new List<Seat>(); // Trả về danh sách trống nếu không tìm thấy khung giờ chiếu
            }

            // Kiểm tra xem đã tạo ghế cho khung giờ chiếu này trước đó chưa
            var existingSeats = _context.Seats.Any(s => s.ScreeningId == screeningId);

            // Nếu đã tồn tại ghế cho khung giờ chiếu này, chỉ cần trả về danh sách các ghế từ cơ sở dữ liệu
            if (existingSeats)
            {
                var seatsFromDatabase = _context.Seats.Where(s => s.ScreeningId == screeningId).ToList();
                return seatsFromDatabase;
            }

            // Nếu chưa có ghế được tạo cho khung giờ chiếu này, tạo mới danh sách ghế và lưu vào cơ sở dữ liệu
            var seats = new List<Seat>();
            for (int row = 1; row <= 8; row++)
            {
                for (int seatNumber = 1; seatNumber <= 12; seatNumber++)
                {
                    // Tạo mã ghế
                    string seatCode = $"{(char)('A' + row - 1)}{seatNumber}";

                    // Tạo đối tượng ghế mới và thêm vào danh sách
                    var seat = new Seat
                    {
                        SeatCode = seatCode,
                        Row = row.ToString(),
                        Number = seatNumber,
                        IsAvailable = true, // Mặc định là ghế trống
                        CinemaId = screening.CinemaId,
                        ScreeningId = screeningId
                    };

                    seats.Add(seat);
                }
            }

            // Lưu danh sách các ghế vào cơ sở dữ liệu
            _context.Seats.AddRange(seats);
            _context.SaveChanges();

            return seats;
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

            //// Truyền dữ liệu qua ViewBag để sử dụng trong TicketDetails
            //ViewBag.SelectedSeats = selectedSeatIds;
            //ViewBag.TotalPrice = totalPrice;

            //return RedirectToAction("TicketDetails", new { screeningId = screeningId, timeSlot = timeSlot });
            //return RedirectToAction("TicketDetails", new { screeningId = screeningId, timeSlot = timeSlot, selectedSeatIds = selectedSeatIds, totalPrice = totalPrice });
            return RedirectToAction("Index", "SeatBookingCart", new { screeningId = screeningId, timeSlot = timeSlot, selectedSeatIds = selectedSeatIds });
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
