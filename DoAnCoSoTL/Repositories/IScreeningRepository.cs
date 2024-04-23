using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IScreeningRepository
    {
        Task InsertScreeningAsync(Screening screening);
        IEnumerable<Cinema> GetSelectedCinemasByMovieId(Guid movieId);
        Task<IEnumerable<Screening>> GetAllAsync();
        Task<IEnumerable<Screening>> GetScreeningsByDate(DateTime date);
    }
}
