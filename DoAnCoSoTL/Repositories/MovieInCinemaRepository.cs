using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSoTL.Repositories
{
    public class MovieInCinemaRepository : IMovieInCinemaRepository
    {
        MovieContext db;
        public MovieInCinemaRepository(MovieContext db)
        {
           this.db = db;
        }

        public async Task<IEnumerable<MovieInCinema>> GetAllAsync()
        {
            return await db.MovieInCinemas.ToListAsync();
        }


        public async Task<MovieInCinema> GetByIdAsync(int id)
        {
            return await db.MovieInCinemas.SingleOrDefaultAsync(p => p.Id == id);

        }

        public async Task InsertAsync(IEnumerable<MovieInCinema> mic)
        {
            await db.MovieInCinemas.AddRangeAsync(mic);
            await db.SaveChangesAsync();
        }

        public async Task Update(int id, MovieInCinema mic)
        {
            var oldMovie = db.MovieInCinemas.SingleOrDefault(w => w.Id == id);
            await db.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var movie = db.MovieInCinemas.SingleOrDefault(w => w.Id == id);
            db.MovieInCinemas.Remove(movie);
            await db.SaveChangesAsync();
        }
    }
}
