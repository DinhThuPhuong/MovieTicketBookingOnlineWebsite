using DoAnCoSoTL.Models;
using DoAnCoSoTL.ViewModels;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieRepository
    {

        Task DeleteAsync(Guid id);
        Task<IEnumerable<Movie>> GetAllAsync();
        //MovieViewModel GetMovieByIdAdmin(Guid id);
        Task <Movie> GetByIdAsync(Guid id);
        //Task<Movie> GetByName(string name);
        Task InsertAsync(Movie movie);
        Task UpdateAsync(Movie editMovie, Guid Mid);


    }
}
