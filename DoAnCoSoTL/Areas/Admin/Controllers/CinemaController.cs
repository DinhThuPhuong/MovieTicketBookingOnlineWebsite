using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSoTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepository;
        MovieContext _db;
        public CinemaController(ICinemaRepository cinemaRepository, MovieContext db)
        {
            _cinemaRepository = cinemaRepository;
            _db = db;

        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay  đổi đường dẫn theo cấu hình của bạn 
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối }

        }
        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            return View(cinemas);
        }
        //public IActionResult Index()
        //{
        //    var actors = _actorRepository.GetAllAsync();
        //    return View(actors);
        //}
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {

            Cinema cinemas = await _cinemaRepository.GetByIdAsync(id);
            return View("Details", cinemas);
        }
        public IActionResult Create()
        {
            return View("Create", new Cinema());
        }

        //action xử lý thêm category
        [HttpPost]
        public async Task<IActionResult> Create(Cinema newCinema, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                if (Image != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    newCinema.Image = await SaveImage(Image);
                }

                await _cinemaRepository.InsertAsync(newCinema);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> Update(int id)
        {
            var cinema = await _cinemaRepository.GetByIdAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }

            // Check if Image is null
            if (cinema.Image != null)
            {
                cinema.Image = "default.jpg"; // Set a default image path or handle accordingly
            }

            return View(cinema);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Cinema cinema, IFormFile Image)
        {
            ModelState.Remove("Image"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != cinema.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var existingCinema = await _cinemaRepository.GetByIdAsync(id);
                if (existingCinema == null)
                {
                    return NotFound();
                }

                // Cập nhật các thông tin khác của cinema
                existingCinema.Name = cinema.Name;
                existingCinema.Description = cinema.Description;
                existingCinema.Location = cinema.Location;
                // Kiểm tra xem có file hình ảnh được tải lên không
                if (Image == null)
                {
                    existingCinema.Image = cinema.Image;
                }
                else
                {
                    // Lưu hình ảnh mới
                    existingCinema.Image = await SaveImage(Image);
                }

                // Cập nhật category vào cơ sở dữ liệu
                await _cinemaRepository.UpdateAsync(existingCinema);
                return RedirectToAction(nameof(Index));
            }

            return View(cinema);
        }
        // Xử lý xóa actor
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cinemaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Keyword)
        {
            ViewData["searching"] = Keyword;
            var cinemas = _db.Cinemas.Select(x => x);
            if (!string.IsNullOrEmpty(Keyword))
            {
                cinemas = cinemas.Where(c => c.Name.Contains(Keyword) || c.Location.Contains(Keyword));

            }
            return View(await cinemas.AsNoTracking().ToListAsync());
        }
    }
}
