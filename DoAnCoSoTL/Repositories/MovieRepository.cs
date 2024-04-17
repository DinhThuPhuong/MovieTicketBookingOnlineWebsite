using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using DoAnCoSoTL.ViewModels;
using Microsoft.EntityFrameworkCore;

public class MovieRepository : IMovieRepository
{
    MovieContext db;
    public MovieRepository(MovieContext _db)
    {
        db = _db;
    }
    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await db.Movies.ToListAsync();
    }
    public async Task<Movie> GetByIdAsync(Guid id)
    {
        return db.Movies.SingleOrDefault(c => c.Id == id);
    }
    //public MovieViewModel GetMovieByIdAdmin(Guid id)
    //{

    //    var movie = db.Movies.SingleOrDefault(c => c.Id == id);

    //    MovieViewModel movieModel = new MovieViewModel();

    //    movieModel.Name = movie.Name;
    //    movieModel.Description = movie.Description;
    //    movieModel.StartDate = movie.StartDate;
    //    movieModel.EndDate = movie.EndDate;
    //    movieModel.Price = movie.Price;
    //    movieModel.Rate = (int)movie.Rate;
    //    return movieModel;
    //}


    //public async Task<Movie> GetByNameAsync(string name)
    //{
    //    return db.Movies.SingleOrDefault(c => c.Name == name);
    //}

    public async Task InsertAsync(Movie movie)
    {
        db.Movies.Add(movie);
        await db.SaveChangesAsync();
        
    }
    public async Task UpdateAsync(Movie editMovie, Guid Mid)
    {
        var movie = db.Movies.SingleOrDefault(c => c.Id == Mid);
        if(movie != null)
        {
            db.Movies.Update(movie);
            await db.SaveChangesAsync();

        }
       
    }
    public async Task DeleteAsync(Guid id)
    {
        var movie = await db.Movies.SingleOrDefaultAsync(c => c.Id == id);
        if(movie != null )
        {
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
        }    
    }
}




