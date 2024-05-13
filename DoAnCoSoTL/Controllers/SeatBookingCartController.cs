using DoAnCoSoTL.Extensions;
using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSoTL.Controllers
{
    [Authorize]
    public class SeatBookingCartController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly MovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SeatBookingCartController(MovieContext context,
        UserManager<ApplicationUser> userManager, IMovieRepository
        movieRepository, ISeatRepository seatRepository)
        {
            _movieRepository = movieRepository;
            _context = context;
            _userManager = userManager;
            _seatRepository = seatRepository;
        }
        //public async Task<IActionResult> AddToCart(int seatId)
        //{
        //    // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
        //    //var seat = await GetProductFromDatabase(seatId);
        //    var seat = await _context.Seats
        //    .Include(s => s.Screening)
        //        .ThenInclude(s => s.Movie)
        //    .Include(s => s.Cinema)
        //    .FirstOrDefaultAsync(s => s.Id == seatId);
        //    var cartItem = new SeatCartItem
        //    {
        //        SeatId = seatId,
        //        MovieName = seat.Screening.Movie.Name,
        //        CinemaName = seat.Cinema.Name,
        //        CinemaLocation = seat.Cinema.Location,
        //        TimeSlot = $"{seat.Screening.Time} - {seat.Screening.EndTime}", 
        //        Price = (decimal)seat.Screening.Movie.Price

        //    };
        //    var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
        //    cart.AddItem(cartItem);
        //    HttpContext.Session.SetObjectAsJson("Cart", cart);
        //    return RedirectToAction("Index");
        //}
        //public async Task<IActionResult> AddToCart(int seatId)
        //{
        //    var seat = await GetProductFromDatabase(seatId);

        //    var cartItem = new SeatCartItem();

        //    if (seat != null && seat.Screening != null && seat.Screening.Movie != null && seat.Cinema != null)
        //    {
        //        cartItem = new SeatCartItem
        //        {
        //            SeatId = seatId,
        //            MovieName = seat.Screening.Movie.Name,
        //            CinemaName = seat.Cinema.Name,
        //            CinemaLocation = seat.Cinema.Location,
        //            TimeSlot = $"{seat.Screening.Time} - {seat.Screening.EndTime}",
        //            Price = (decimal)seat.Screening.Movie.Price
        //        };
        //    }
        //    else
        //    {
        //        // Xử lý trường hợp các đối tượng là null
        //        // Ví dụ: Gán giá trị mặc định hoặc thông báo lỗi
        //        TempData["ErrorMessage"] = "Không thể thêm vé vào giỏ hàng. Vui lòng thử lại sau.";
        //        return RedirectToAction("Index");
        //    }

        //    var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
        //    cart.AddItem(cartItem);
        //    HttpContext.Session.SetObjectAsJson("Cart", cart);

        //    return RedirectToAction("Index");
        //}
        //public async Task<IActionResult> AddToCart(int seatId)
        //{
        //    var seat = await _seatRepository.GetByIdAsync(seatId);

        //    var cartItem = new SeatCartItem();

        //    if (seat != null && seat.Screening != null && seat.Screening.Movie != null && seat.Cinema != null)
        //    {
        //        cartItem = new SeatCartItem
        //        {
        //            SeatId = seatId,
        //            MovieName = seat.Screening.Movie.Name,
        //            CinemaName = seat.Cinema.Name,
        //            CinemaLocation = seat.Cinema.Location,
        //            TimeSlot = $"{seat.Screening.Time} - {seat.Screening.EndTime}",
        //            SeatCode = seat.SeatCode,
        //            Price = (decimal)seat.Screening.Movie.Price
        //        };
        //    }
        //    else
        //    {
        //        // Xử lý trường hợp các đối tượng là null
        //        // Ví dụ: Gán giá trị mặc định hoặc thông báo lỗi
        //        TempData["ErrorMessage"] = "Không thể thêm vé vào giỏ hàng. Vui lòng thử lại sau.";
        //        return RedirectToAction("Index");
        //    }

        //    var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
        //    cart.AddItem(cartItem);
        //    HttpContext.Session.SetObjectAsJson("Cart", cart);

        //    return RedirectToAction("Index");
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddToCart(int seatId)
        //{
        //    var seat = await _seatRepository.GetByIdAsync(seatId);

        //    if (seat != null)
        //    {
        //        var cartItem = new SeatCartItem
        //        {
        //            SeatId = seatId,
        //            MovieName = seat.Screening.Movie.Name,
        //            CinemaName = seat.Cinema.Name,
        //            CinemaLocation = seat.Cinema.Location,
        //            TimeSlot = $"{seat.Screening.Time} - {seat.Screening.EndTime}",
        //            SeatCode = seat.SeatCode,
        //            Price = (decimal)seat.Screening.Movie.Price
        //        };

        //        var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
        //        cart.AddItem(cartItem);
        //        HttpContext.Session.SetObjectAsJson("Cart", cart);
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Không thể thêm vé vào giỏ hàng. Vui lòng thử lại sau.";
        //    }

        //    return RedirectToAction("Choose", "Seat");
        //}
        public async Task<IActionResult> AddToCart(int seatId)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var seat = await _seatRepository.GetByIdAsync(seatId);


            var cartItem = new SeatCartItem
            {
                SeatId = seatId,
                MovieName = seat.Screening.Movie.Name,
                CinemaName = seat.Cinema.Name,
                CinemaLocation = seat.Cinema.Location,
                TimeSlot = $"{seat.Screening.Time} - {seat.Screening.EndTime}",
                SeatCode = seat.SeatCode,
                Price = (decimal)seat.Screening.Movie.Price
            };
            var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart") ?? new SeatBookingCart();
            return View(cart);
        }

        // Các actions khác...

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Hãy thêm vé xem phim vào giỏ hàng trước khi tiếp tục.";
                //return RedirectToAction("Index");
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Hãy thêm vé xem phim vào giỏ hàng trước khi tiếp tục.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            

            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price );
            
            
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                SeatId = i.SeatId,
                MovieName = i.MovieName,
                CinemaName = i.CinemaName,
                TimeSlot = i.TimeSlot,
                CinemaLocation = i.CinemaLocation,
                SeatCode = i.SeatCode,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
        }


        private async Task<Seat> GetProductFromDatabase(int seatId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var seat = await _seatRepository.GetByIdAsync(seatId);
            return seat;
        }
        public IActionResult RemoveFromCart(int seatId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<SeatBookingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(seatId);

                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
    }
}
