using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSoTL.Controllers
{
    public class ScreeningController : Controller
    {
        private readonly MovieContext _context;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMovieRepository _movieRepository;

        public ScreeningController(MovieContext context, ICinemaRepository cinemaRepository, IMovieRepository movieRepository)
        {
            _context = context;
            _cinemaRepository = cinemaRepository;
            _movieRepository = movieRepository;
        }

        // GET: Screening/Index
        public async Task<IActionResult> Index(Guid movieId)
        {
            var screenings = _context.Screenings.Where(s => s.MovieId == movieId).ToList();
            foreach (var screening in screenings)
            {
                screening.Movie =  await _movieRepository.GetByIdAsync(screening.MovieId);
                screening.Cinema = await _cinemaRepository.GetByIdAsync(screening.CinemaId);

            }

            return View(screenings);
        }

        // GET: Screening/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }
    }
}
