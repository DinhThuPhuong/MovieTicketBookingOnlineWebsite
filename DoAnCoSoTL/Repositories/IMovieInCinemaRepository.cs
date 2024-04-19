using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieInCinemaRepository
    {
        Task<IEnumerable<MovieInCinema>> GetAllAsync();
        Task <MovieInCinema> GetByIdAsync(int id);
        Task InsertAsync(IEnumerable<MovieInCinema> mic);
        Task Update(int id,MovieInCinema mic);
        Task Delete(int id);
    }
}
