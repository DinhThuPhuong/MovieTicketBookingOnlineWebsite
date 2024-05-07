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

        // GET: Seat/Choose
        public IActionResult Choose(int screeningId, string timeSlot)
        {
            // Truyền screeningId và timeSlot đến trang chọn ghế
            ViewData["ScreeningId"] = screeningId;
            ViewData["TimeSlot"] = timeSlot;

            return View();
        }
    }
}
