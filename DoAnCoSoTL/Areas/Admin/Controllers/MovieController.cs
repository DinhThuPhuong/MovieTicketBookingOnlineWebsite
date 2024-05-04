using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using DoAnCoSoTL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Principal;

namespace DoAnCoSoTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class MovieController : Controller
    {
        private readonly IMovieActorRepository _movieActorRepository;
        private readonly IMovieInCinemaRepository _moviesInCinemaRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieRepository _movieRepository;
        MovieContext _db;
        public MovieController(ICategoryRepository categoryRepository, IMovieRepository movieRepository, MovieContext db,
            IMovieActorRepository movieactorRepository,
            ICinemaRepository cinemaRepository, IProducerRepository producerRepository, IActorRepository actorRepository, IMovieInCinemaRepository movieInCinemaRepository)
        {
            _categoryRepository = categoryRepository;
            _db = db;
            _movieRepository = movieRepository;
            _movieActorRepository = movieactorRepository;
            _producerRepository = producerRepository;
            _actorRepository = actorRepository;
            _cinemaRepository = cinemaRepository;
            _moviesInCinemaRepository = movieInCinemaRepository;
        }
        static Guid iid;
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllAsync();
            return View(movies);
        }
        private string GetMovieStatus(DateTime releaseDate)
        {
            DateTime currentDate = DateTime.Now;

            // Số ngày tối đa mà phim được xem là phim sắp ra mắt
            int daysUntilRelease = 7;

            // Kiểm tra nếu ngày hiện tại lớn hơn ngày khởi chiếu
            if (currentDate > releaseDate)
            {
                return "Đang chiếu";
            }
            // Kiểm tra nếu ngày hiện tại gần hơn một số ngày nhất định so với ngày khởi chiếu
            else if ((releaseDate - currentDate).TotalDays <= daysUntilRelease)
            {
                return "Sắp ra mắt";
            }
            else
            {
                return "Sắp chiếu";
            }
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
        public IActionResult Create()
        {
            ViewBag.Cinemas = new SelectList(_db.Cinemas.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
            ViewBag.Actors = new SelectList(_db.Actors.ToList(), "Id", "Name");
            ViewBag.Producers = new SelectList(_db.Producers.ToList(), "Id", "Name");

            return View(new MovieViewModel());
        }
        //action xử lý thêm movie

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movievm, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var movie = new Movie
                    {
                        Id = Guid.NewGuid(),
                        Name = movievm.Name,
                        Description = movievm.Description,
                        Price = movievm.Price,
                        Rate = movievm.Rate,
                        StartDate = movievm.StartDate,
                        EndDate = movievm.EndDate,
                        Cat_Id = movievm.Category_Id,
                        Producer_Id = movievm.Producer_Id,
                        Trailer = movievm.Trailer,
                        DurationMinutes = movievm.DurationMinutes,
                    };

                    if (Image != null)
                    {
                        movie.Image = await SaveImage(Image);
                    }

                    await _movieRepository.InsertAsync(movie);

                    if (movievm.ActorIds != null && movievm.ActorIds.Any())
                    {
                        foreach (var actorId in movievm.ActorIds)
                        {
                            await _movieActorRepository.InsertMovieActorAsync(movie.Id, actorId);
                        }
                    }

                    if (movievm.CinemaIds != null && movievm.CinemaIds.Any())
                    {
                        foreach (var cinemaId in movievm.CinemaIds)
                        {
                            await _moviesInCinemaRepository.InsertMoviesInCinemaAsync(movie.Id, cinemaId);
                        }
                    }

                    return RedirectToAction("Index", "Movie");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ hoặc có lỗi, bạn có thể hiển thị lại view Create với dữ liệu hiện tại và thông báo lỗi
            ViewBag.Cinemas = new SelectList(_db.Cinemas.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
            ViewBag.Actors = new SelectList(_db.Actors.ToList(), "Id", "Name");
            ViewBag.Producers = new SelectList(_db.Producers.ToList(), "Id", "Name");
            return View("Create", movievm);
        }




        public ActionResult Update(Guid id)

        {
            iid = id;
            MovieViewModel Moviemodel = _movieRepository.GetMovieByIdAdmin(id);

            ViewBag.Cinemas = new SelectList(_db.Cinemas.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
            ViewBag.Actors = new SelectList(_db.Actors.ToList(), "Id", "Name");
            ViewBag.Producers = new SelectList(_db.Producers.ToList(), "Id", "Name");



            return View("Update", Moviemodel);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Update(MovieViewModel editMovie, IFormFile Image)
        {

            _movieRepository.Update(editMovie, iid, Image);
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Update(Guid id)

        //{
        //    iid = id;
        //    MovieViewModel Moviemodel = await _movieRepository.GetMovieByIdAdmin(id);


        //    ViewBag.Cinemas = new SelectList(_db.Cinemas.ToList(), "Id", "Name");
        //    ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
        //    ViewBag.Actors = new SelectList(_db.Actors.ToList(), "Id", "Name");
        //    ViewBag.Producers = new SelectList(_db.Producers.ToList(), "Id", "Name");



        //    return View("Update", Moviemodel);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(Guid id, MovieViewModel movievm, IFormFile Image)
        //{
        //    id = iid;
        //    var movie = await _movieRepository.GetByIdAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }
        //    ModelState.Remove("Image");
        //    if (!ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //var movie = await _movieRepository.GetByIdAsync(id);
        //            //if (movie == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            movie.Name = movievm.Name;
        //            movie.Description = movievm.Description;
        //            movie.Price = movievm.Price;
        //            movie.Rate = movievm.Rate;
        //            movie.StartDate = movievm.StartDate;
        //            movie.EndDate = movievm.EndDate;
        //            movie.Cat_Id = movievm.Category_Id;
        //            movie.Producer_Id = movievm.Producer_Id;
        //            movie.Trailer = movievm.Trailer;
        //            movie.DurationMinutes = movievm.DurationMinutes;
        //            if (Image != null)
        //            {
        //                movie.Image = await SaveImage(Image);
        //            }

        //            await _movieRepository.UpdateAsync(movie);

        //            // Xóa tất cả các liên kết diễn viên và rạp chiếu phim cũ
        //            await _movieActorRepository.DeleteByMovieIdAsync(id);
        //            await _moviesInCinemaRepository.DeleteByMovieIdAsync(id);

        //            // Thêm các liên kết diễn viên mới
        //            if (movievm.ActorIds != null && movievm.ActorIds.Any())
        //            {
        //                foreach (var actorId in movievm.ActorIds)
        //                {
        //                    await _movieActorRepository.InsertMovieActorAsync(movie.Id, actorId);
        //                }
        //            }

        //            // Thêm các liên kết rạp chiếu phim mới
        //            if (movievm.CinemaIds != null && movievm.CinemaIds.Any())
        //            {
        //                foreach (var cinemaId in movievm.CinemaIds)
        //                {
        //                    await _moviesInCinemaRepository.InsertMoviesInCinemaAsync(movie.Id, cinemaId);
        //                }
        //            }

        //            return RedirectToAction("Index", "Movie");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, "An error occurred while updating data: " + ex.Message);
        //        }
        //    }

        //    // Nếu ModelState không hợp lệ hoặc có lỗi, bạn có thể hiển thị lại view Update với dữ liệu hiện tại và thông báo lỗi
        //    ViewBag.Cinemas = new SelectList(_db.Cinemas.ToList(), "Id", "Name");
        //    ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
        //    ViewBag.Actors = new SelectList(_db.Actors.ToList(), "Id", "Name");
        //    ViewBag.Producers = new SelectList(_db.Producers.ToList(), "Id", "Name");
        //    return View("Update", movievm);
        //}



        public async Task<IActionResult> Delete(Guid id)
        {
            await _movieRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            Movie movies = await _movieRepository.GetByIdAsync(id);
            return View("Details", movies);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string Keyword)
        {
            ViewData["searching"] = Keyword;
            var movies = _db.Movies.Select(x => x);
            if (!string.IsNullOrEmpty(Keyword))
            {
                movies = movies.Where(c => c.Name.Contains(Keyword));
            }
            var moviesList = await movies.AsNoTracking().ToListAsync();
            return View(moviesList);
        }

    }
}
