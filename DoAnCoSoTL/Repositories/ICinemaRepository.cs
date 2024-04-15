using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface ICinemaRepository
    {
        int delete(int id);
        List<Cinema> GetAll();
        Cinema GetById(int id);
        Cinema GetByLocation(string location);
        Cinema GetByName(string name);
       Task<int> insert(Cinema newCinema, List<IFormFile> Image);
        Task<int> update(Cinema EditCin, int id, List<IFormFile> Image);
    }
}