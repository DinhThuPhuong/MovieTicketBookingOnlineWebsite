using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSoTL.Controllers
{
	public class OrderController : Controller
	{
		public IActionResult OrderDetails(string movieName, string timeSlot, DateTime screeningDate, string cinemaName, List<string> selectedSeats, double totalPrice)
		{
			// Truyền các thông tin cần thiết từ trang chọn ghế đến trang chi tiết đơn hàng
			ViewData["MovieName"] = movieName;
			ViewData["TimeSlot"] = timeSlot;
			ViewData["ScreeningDate"] = screeningDate;
			ViewData["CinemaName"] = cinemaName;
			ViewData["SelectedSeats"] = selectedSeats;
			ViewData["TotalPrice"] = totalPrice;

			return View();
		}
	}
}
