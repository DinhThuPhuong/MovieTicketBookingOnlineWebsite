using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieInCinemaRepository
    {
        Task<IEnumerable<MoviesInCinema>> GetAllAsync();
        Task<MoviesInCinema> GetByIdAsync(int id);
       // Task InsertAsync(IEnumerable<MoviesInCinema> mic);
        Task Update(int id, MoviesInCinema mic);
        Task Delete(int id);
        Task InsertMoviesInCinemaAsync(Guid movieId, int cinemaId);
        Task DeleteByMovieIdAsync(Guid id);
    }
}
