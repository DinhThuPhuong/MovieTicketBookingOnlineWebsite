﻿using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSoTL.Controllers
{
    public class OrderListController : Controller
    {
        private readonly MovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        public OrderListController(IOrderRepository orderRepository, MovieContext context, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User); // nơi lưu trữ thông tin về người dùng hiện tại đang tương tác với ứng dụng.
                                                                          //trả về một đối tượng người dùng (User) từ UserManager, đại diện cho người dùng hiện tại
                                                                          // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (user != null)
            {
                // Truy vấn các đơn hàng của người dùng dựa trên UserId
                var orders = await _context.Orders
                    .Where(o => o.ApplicationUser.Id == user.Id)
                    .ToListAsync();
                return View(orders);
            }
            else
            {
                // Người dùng chưa đăng nhập, có thể xử lý tùy ý
                // Ví dụ: Redirect người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Display(int id)
        {
            //OrderDetails có một mối quan hệ với Product thông qua một khóa ngoại
            // tạo một truy vấn duy nhất để lấy dữ liệu từ cả hai bảng OrderDetails và Product
            var order = _context.OrderDetails.Where(od => od.OrderId == id)
                                                    .Include(od => od.Movie)
                                                    .ToList();

            return View(order);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Lấy tất cả các đơn hàng từ cơ sở dữ liệu
            var orders = await _context.OrderDetails
                                        .Include(od => od.Movie)
                                        .ToListAsync();

            if (orders == null || !orders.Any())
            {
                // Nếu không tìm thấy đơn hàng, trả về trang 404 Not Found
                return NotFound();
            }

            // Trả về view chi tiết với danh sách tất cả các đơn hàng đã lấy được
            return View(orders);
        }


        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        // Xử lý cập nhật sản phẩm
        //[HttpPost]
        //public async Task<IActionResult> Update(int id, Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        await _orderRepository.UpdateAsync(order); //Truyền vào 1 order và nó sẽ đc lưu vào DB
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(order);
        //}

        [HttpPost]
        public async Task<IActionResult> Update(int id, Order order)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Lấy thời gian hiện tại ở định dạng UTC
            var currentTimeUtc = DateTime.UtcNow;

            // Tính toán khoảng cách thời gian giữa thời gian hiện tại và thời gian đặt hàng của đơn đặt hàng
            var timeDifference = currentTimeUtc - existingOrder.OrderDate;

            // Kiểm tra xem thời gian đặt hàng có chưa quá 1 ngày hay không
            if (timeDifference.TotalDays >= 1)
            {
                // Nếu đã quá 1 ngày, từ chối cập nhật và hiển thị thông báo cho người dùng
                ModelState.AddModelError(string.Empty, "Bạn không thể cập nhật đơn hàng đã đặt hơn 1 ngày.");
                return View(order);
            }

            // Kiểm tra sự khác biệt giữa Id trong URL và Id của đơn hàng được gửi
            if (id != order.Id)
            {
                return NotFound();
            }

            
            if (!ModelState.IsValid)
            {
                await _orderRepository.UpdateAsync(existingOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Lấy thời gian hiện tại ở định dạng UTC
            var currentTimeUtc = DateTime.UtcNow;

            // Tính toán khoảng cách thời gian giữa thời gian hiện tại và thời gian đặt hàng của đơn đặt hàng
            var timeDifference = currentTimeUtc - existingOrder.OrderDate;

            // Kiểm tra xem thời gian đặt hàng có chưa quá 1 ngày hay không
            if (timeDifference.TotalDays >= 1)
            {
                // Nếu đã quá 1 ngày, từ chối xóa và hiển thị thông báo cho người dùng
                ModelState.AddModelError(string.Empty, "Bạn không thể xóa đơn hàng đã đặt hơn 1 ngày.");
                return View(existingOrder);
            }

            // Nếu thời gian chưa quá 1 ngày, cho phép xóa
            return View(existingOrder);
        }


        // Xử lý xóa sản phẩm
        [HttpPost/*, ActionName("Delete")*/]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
