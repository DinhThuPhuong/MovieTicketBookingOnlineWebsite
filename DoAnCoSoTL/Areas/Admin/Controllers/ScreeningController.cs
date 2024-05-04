using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
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
        private readonly IMovieInCinemaRepository _cinemainCinemaRepository;
        private readonly MovieContext _db;

        public ScreeningController(IScreeningRepository screeningRepository, IMovieRepository movieRepository, ICinemaRepository cinemaRepository, MovieContext db, IMovieInCinemaRepository cinemainCinemaRepository)
        {
            _screeningRepository = screeningRepository;
            _movieRepository = movieRepository;
            _cinemaRepository = cinemaRepository;
            _db = db;
            _cinemainCinemaRepository = cinemainCinemaRepository;
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
                var movieDurationMinutes = movies.First().DurationMinutes;
                ViewBag.MovieDurationMinutes = movieDurationMinutes;
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
            if (!ModelState.IsValid)
            {
                try
                {
                    // Lấy ngày bắt đầu và kết thúc của Movie
                    var moviek = await _movieRepository.GetAllAsync();
                    var movie = await _movieRepository.GetByIdAsync(screening.MovieId);
                    if (movie == null)
                    {
                        // Xử lý trường hợp movie không tồn tại
                        ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin phim");
                        return View(screening);
                    }
                    else
                    {
                        // Kiểm tra ngày hợp lệ khi movie tồn tại
                        if (screening.Date < movie.StartDate || screening.Date > movie.EndDate)
                        {
                            ModelState.AddModelError(string.Empty, "Ngày chiếu phim không hợp lệ");
                           
                            ViewBag.Movies = new SelectList(moviek, "Id", "Name");
                            return View(screening);
                        }
                    }


                    var screeninglist = _screeningRepository.GetByMovieId(screening.MovieId).Result;
                    foreach (var item in screeninglist)
                    {
                        if (item.CinemaId == screening.CinemaId && item.Time == screening.Time && item.Date == screening.Date)
                        {
                            ModelState.AddModelError(string.Empty, "Lịch chiếu đã tồn tại");
             
                            ViewBag.Movies = new SelectList(moviek, "Id", "Name");
                            return View(screening);
                        }
                    }
                    screening.Movie = movie;
                   
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
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // Lấy thông tin của buổi chiếu từ cơ sở dữ liệu
            var screening = await _screeningRepository.GetByIdAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            // Lấy danh sách rạp chiếu và danh sách phim từ cơ sở dữ liệu
            var cinemas = await _cinemaRepository.GetAllAsync();
            var movies = await _movieRepository.GetAllAsync();

            // Trả về view với thông tin của buổi chiếu và danh sách rạp chiếu, danh sách phim
            ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name", screening.CinemaId);
            ViewBag.Movies = new SelectList(movies, "Id", "Name", screening.MovieId);

            return View(screening);
        }

        //// Xử lý cập nhật sản phẩm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int id, Screening screening)
        //{
        //    if (id != screening.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Lấy đối tượng Screening hiện tại từ cơ sở dữ liệu
        //            var moviek = await _movieRepository.GetAllAsync();
        //            var movie = await _movieRepository.GetByIdAsync(screening.MovieId);
        //            if (movie == null)
        //            {
        //                // Xử lý trường hợp movie không tồn tại
        //                ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin phim");
        //                return View(screening);
        //            }
        //            else
        //            {
        //                // Kiểm tra ngày hợp lệ khi movie tồn tại
        //                if (screening.Date < movie.StartDate || screening.Date > movie.EndDate)
        //                {
        //                    ModelState.AddModelError(string.Empty, "Ngày chiếu phim không hợp lệ");

        //                    ViewBag.Movies = new SelectList(moviek, "Id", "Name");
        //                    return View(screening);
        //                }
        //            }


        //            var screeninglist = _screeningRepository.GetByMovieId(screening.MovieId).Result;
        //            foreach (var item in screeninglist)
        //            {
        //                if (item.CinemaId == screening.CinemaId && item.Time == screening.Time && item.Date == screening.Date)
        //                {
        //                    ModelState.AddModelError(string.Empty, "Lịch chiếu đã tồn tại");

        //                    ViewBag.Movies = new SelectList(moviek, "Id", "Name");
        //                    return View(screening);
        //                }
        //            }
        //            // Lấy đối tượng Screening hiện tại từ cơ sở dữ liệu
        //            var existingScreening = await _screeningRepository.GetByIdAsync(id);
        //            if (existingScreening == null)
        //            {
        //                return NotFound();
        //            }

        //            // Cập nhật thông tin của đối tượng Screening
        //            existingScreening.Time = screening.Time;
        //            existingScreening.CinemaId = screening.CinemaId;
        //            existingScreening.MovieId = screening.MovieId;
        //            existingScreening.EndTime = screening.EndTime;
        //            await _screeningRepository.UpdateScreeningAsync(existingScreening);

        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
        //        }
        //    }
        //    // Lấy danh sách rạp chiếu và danh sách phim từ cơ sở dữ liệu
        //    var cinemas = await _cinemaRepository.GetAllAsync();
        //    var movies = await _movieRepository.GetAllAsync();

        //    // Trả về view với đối tượng Screening và danh sách rạp chiếu, danh sách phim
        //    ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
        //    ViewBag.Movies = new SelectList(movies, "Id", "Name");
        //    // Nếu ModelState không hợp lệ, trả về view với dữ liệu hiện tại
        //    return View(screening);
        //}
        [HttpPost]
        public async Task<IActionResult> Update(int id, Screening screening)
        {
            if (id != screening.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // Lấy đối tượng Screening hiện tại từ cơ sở dữ liệu
                    var existingScreening = await _screeningRepository.GetByIdAsync(id);

                    if (existingScreening == null)
                    {
                        return NotFound();
                    }

                    var moviek = await _movieRepository.GetAllAsync();
                    var movie = await _movieRepository.GetByIdAsync(screening.MovieId);
                    if (movie == null)
                    {
                        // Xử lý trường hợp movie không tồn tại
                        ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin phim");
                        return View(screening);
                    }
                    else
                    {
                        // Kiểm tra ngày hợp lệ khi movie tồn tại
                        if (screening.Date < movie.StartDate || screening.Date > movie.EndDate)
                        {
                            ModelState.AddModelError(string.Empty, "Ngày chiếu phim không hợp lệ");

                            ViewBag.Movies = new SelectList(moviek, "Id", "Name");
                            return View(screening);
                        }
                    }


                    var screeninglist = _screeningRepository.GetByMovieId(screening.MovieId).Result;
                    foreach (var item in screeninglist)
                    {
                        if (item.CinemaId == screening.CinemaId && item.Time == screening.Time && item.Date == screening.Date)
                        {
                            ModelState.AddModelError(string.Empty, "Lịch chiếu đã tồn tại");

                            ViewBag.Movies = new SelectList(moviek, "Id", "Name");
                            return View(screening);
                        }
                    }
                    // Lấy đối tượng Screening hiện tại từ cơ sở dữ liệu
                 


                    // Cập nhật thông tin của đối tượng Screening
                    existingScreening.Time = screening.Time;
                    existingScreening.CinemaId = screening.CinemaId;
                    existingScreening.MovieId = screening.MovieId;
                    existingScreening.EndTime = screening.EndTime;
                    await _screeningRepository.UpdateScreeningAsync(existingScreening);

                    return RedirectToAction(nameof(Index));
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
            // Nếu ModelState không hợp lệ, trả về view với dữ liệu hiện tại
            return View(screening);
        }





        [HttpGet]
        public async Task<IActionResult> GetFilmDates(Guid movieId)
        {
            try
            {
                var movie = await _movieRepository.GetByIdAsync(movieId);
                if (movie != null)
                {
                    return Json(new { startDate = movie.StartDate.ToString("yyyy-MM-dd"), endDate = movie.EndDate.ToString("yyyy-MM-dd") });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving film dates: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieDuration(Guid movieId)
        {
            try
            {
                // Tìm bộ phim trong cơ sở dữ liệu dựa trên movieId
                var movie = await _movieRepository.GetByIdAsync(movieId);

                // Nếu bộ phim tồn tại, trả về thời lượng của bộ phim
                if (movie != null)
                {
                    return Ok(movie.DurationMinutes);
                }

                // Nếu không tìm thấy bộ phim, trả về NotFound
                return NotFound();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, trả về lỗi Internal Server Error
                return StatusCode(500, $"An error occurred while retrieving movie duration: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectedCinemas(Guid movieId)
        {
            var selectedCinemas = await _cinemainCinemaRepository.GetCinemasByMovieId(movieId);
            return Json(selectedCinemas);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _screeningRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {

            var screening = await _screeningRepository.GetByIdAsync(id);
            if (screening.MovieId != null && screening.CinemaId != null)
            {
                screening.Movie = await _movieRepository.GetByIdAsync(screening.MovieId);
                screening.Cinema = await _cinemaRepository.GetByIdAsync(screening.CinemaId);
                return View("Details", screening);
            }
            return RedirectToAction("Index");

        }

        //return View("Details", screening);




        // Other necessary actions
    }
}

