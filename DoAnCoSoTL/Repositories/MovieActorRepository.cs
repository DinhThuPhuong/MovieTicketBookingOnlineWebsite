using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{ 
    public class MovieActorRepository : IMovieActorRepository
    {
        MovieContext db;
        public MovieActorRepository(MovieContext db)
        {
           this.db = db;
        }
        public List<MovieActor> GetAll()
        {
            var MovieActor = db.MovieActors.ToList();
            return MovieActor;
        }
    }
}
