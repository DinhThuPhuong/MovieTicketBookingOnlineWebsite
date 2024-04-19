using DoAnCoSoTL.Models;
using DoAnCoSoTL.ViewModels;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> GetByNameAsync(string name);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task InsertAsync( MovieViewModel movievm, IFormFile Image);
        Task UpdateAsync(Movie editMovie);
        Task DeleteAsync(int id);

    }
}