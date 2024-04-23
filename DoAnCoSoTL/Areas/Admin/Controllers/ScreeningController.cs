using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnCoSoTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ScreeningController : Controller
    {
        private readonly IScreeningRepository _screeningRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly MovieContext _db;

        public ScreeningController(IScreeningRepository screeningRepository, IMovieRepository movieRepository, ICinemaRepository cinemaRepository, MovieContext db)
        {
            _screeningRepository = screeningRepository;
            _movieRepository = movieRepository;
            _cinemaRepository = cinemaRepository;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var screenings = await _screeningRepository.GetAllAsync();

            // Thực hiện eager loading cho thuộc tính Cinema
            foreach (var screening in screenings)
            {
                await _db.Entry(screening)
                    .Reference(s => s.Cinema)
                    .LoadAsync();
            }

            // Thực hiện eager loading cho thuộc tính Movie
            foreach (var screening in screenings)
            {
                await _db.Entry(screening)
                    .Reference(s => s.Movie)
                    .LoadAsync();
            }

            return View(screenings);
        }




        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Tạo một đối tượng Screening mới để truyền vào view
            var screening = new Screening();

            // Lấy danh sách rạp chiếu và danh sách phim từ cơ sở dữ liệu
            var cinemas = await _cinemaRepository.GetAllAsync();
            var movies = await _movieRepository.GetAllAsync();

            // Kiểm tra xem có ít nhất một phim tồn tại trong danh sách phim không
            if (movies.Any())
            {
                // Chọn một phim mặc định để hiển thị trên view
                screening.MovieId = movies.First().Id;
            }

            // Truyền danh sách rạp chiếu và danh sách phim vào ViewBag
            ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            // Trả về view với đối tượng Screening mới
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Screening screening)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy ngày bắt đầu và kết thúc của Movie
                    var movie = await _movieRepository.GetByIdAsync(screening.MovieId);
                    if (movie == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Movie reference.");
                        return View(screening);
                    }

                    screening.Movie = movie;

                    // Tính toán thời gian kết thúc dựa trên thời gian bắt đầu và thời lượng của phim
                    screening.EndTime = screening.Time.Add(TimeSpan.FromMinutes(movie.DurationMinutes));

                    // Lưu đối tượng Screening vào cơ sở dữ liệu
                    await _screeningRepository.InsertScreeningAsync(screening);
                    return RedirectToAction("Index", "Screening");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                }
            }

            // Lấy danh sách rạp chiếu và danh sách phim từ cơ sở dữ liệu
            var cinemas = await _cinemaRepository.GetAllAsync();
            var movies = await _movieRepository.GetAllAsync();

            // Trả về view với đối tượng Screening và danh sách rạp chiếu, danh sách phim
            ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            return View(screening);
        }



        [HttpPost]
        public IActionResult GetSelectedCinemas(Guid movieId)
        {
            var selectedCinemas = _screeningRepository.GetSelectedCinemasByMovieId(movieId);
            return Json(selectedCinemas);
        }

        // Other necessary actions
    }
}

