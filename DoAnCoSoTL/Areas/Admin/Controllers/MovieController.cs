using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using DoAnCoSoTL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DoAnCoSoTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class MovieController : Controller
    {
        private readonly IMovieActorRepository _movieactorRepository;
        private readonly IMovieInCinemaRepository _movieincinemaRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieRepository _movieRepository;
        MovieContext _db;
        public MovieController(ICategoryRepository categoryRepository, IMovieRepository movieRepository, MovieContext db, 
            IMovieActorRepository movieactorRepository, IMovieInCinemaRepository movieincinemaRepository, 
            ICinemaRepository cinemaRepository, IProducerRepository producerRepository, IActorRepository actorRepository)
        {
            _categoryRepository = categoryRepository;
            _db = db;
            _movieRepository = movieRepository;
            _movieactorRepository = movieactorRepository;
            _movieincinemaRepository = movieincinemaRepository;
            _producerRepository = producerRepository;
            _actorRepository = actorRepository;
            _cinemaRepository = cinemaRepository;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllAsync();
            return View(movies);
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
        //action xử lý thêm category
        [HttpPost]

        public async Task<IActionResult> Create(MovieViewModel newMovie, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                if (Image != null)
                {
                    // Lưu hình ảnh đại diện
                    newMovie.Image = await SaveImage(Image);
                }

                await _movieRepository.InsertAsync(newMovie, Image);
                return RedirectToAction(nameof(Index));
            }

            // Load danh sách rạp chiếu phim, danh mục, diễn viên và nhà sản xuất để hiển thị trong dropdownlist
            ViewBag.Cinemas = new SelectList(_cinemaRepository.GetAllAsync().Result, "Id", "Name");
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllAsync().Result, "Id", "Name");
            ViewBag.Actors = new SelectList(_actorRepository.GetAllAsync().Result, "Id", "Name");
            ViewBag.Producers = new SelectList(_producerRepository.GetAllAsync().Result, "Id", "Name");

            return View(newMovie);
        }



        public async Task<IActionResult> Update(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            // Check if Image is null
            if (movie.Image != null)
            {
                movie.Image = "default.jpg"; // Set a default image path or handle accordingly
            }

            return View(movie);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int Mid, MovieViewModel movieVM, IFormFile imageUrl)
        //{
        //    var movie = _db.Movies.SingleOrDefault(c => c.Id == Mid);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }
        //    ModelState.Remove("ImageUrl");
        //    if (ModelState.IsValid)
        //    {
        //        var existingMovie = await _movieRepository.GetByIdAsync(Mid);


        //        existingMovie.Name = movieVM.Name;
        //        existingMovie.Id = Mid;
        //        existingMovie.Description = movieVM.Description;
        //        existingMovie.StartDate = movieVM.StartDate;
        //        existingMovie.EndDate = movieVM.EndDate;
        //        existingMovie.Price = movieVM.Price;
        //        existingMovie.Rate = movieVM.Rate;
        //        existingMovie.Cat_Id = movieVM.Category_Id;
        //        existingMovie.Producer_Id = movieVM.Producer_Id;
        //        existingMovie.Trailer = movieVM.Trailer;
        //        if (imageUrl != null)
        //        {
        //            existingMovie.Image = await SaveImage(imageUrl);
        //        }

        //        if (movieVM.ActorIds != null)
        //        {
        //            foreach (var id in movieVM.ActorIds)
        //            {
        //                _db.MovieActors.Update(new MovieActor()
        //                {
        //                    MovieId = Mid,
        //                    ActorId = id
        //                });
        //            }
        //        }
        //        //adding to cinema movies table
        //        if (movieVM.CinemaIds != null)
        //        {
        //            foreach (var id in movieVM.CinemaIds)
        //            {
        //                _db.MovieInCinemas.Add(new MovieInCinema()
        //                {
        //                    MovieId = Mid,
        //                    CinemaId = id
        //                });
        //            }
        //        }
        //        await _movieRepository.UpdateAsync(existingMovie);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    //Nếu ModelState không hợp lệ, cần cung cấp lại dữ liệu cho view
        //    ViewBag.Actors = await _actorRepository.GetAllAsync();
        //    ViewBag.Cinemas = await _cinemaRepository.GetAllAsync();
        //    ViewBag.Categories = await _categoryRepository.GetAllAsync();
        //    ViewBag.Producers = await _producerRepository.GetAllAsync();

        //    return View(movieVM);
        //}
        // Xử lý xóa sản phẩm
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _movieRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
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