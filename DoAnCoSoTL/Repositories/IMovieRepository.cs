using DoAnCoSoTL.Models;
using DoAnCoSoTL.ViewModels;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieRepository
    {

        int delete(Guid id);
        List<Movie> GetAll();
        MovieViewModel GetMovieByIdAdmin(Guid id);
        Movie GetById(Guid id);
        Movie GetByName(string name);
        Task<int> Insert(MovieViewModel newCinema, List<IFormFile> Image);
        Task<int> update(MovieViewModel editMovie, Guid id, List<IFormFile> Image);


    }
}
