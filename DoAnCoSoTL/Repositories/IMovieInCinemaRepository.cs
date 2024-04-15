using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IMovieInCinemaRepository
    {
        public List<MovieInCinema> GetAll();
        public MovieInCinema GetById(int id);
        public void Insert(List<MovieInCinema> mic);
        public void Update(int id,MovieInCinema mic);
        public void Delete(int id);
    }
}
