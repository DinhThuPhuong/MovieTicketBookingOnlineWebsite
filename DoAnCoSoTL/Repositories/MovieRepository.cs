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
    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await db.Movies.ToListAsync();
    }
    public async Task<Movie> GetByIdAsync(Guid id)
    {
        return await db.Movies.FindAsync(id);
    }
    //public async Task<MovieViewModel> GetMovieByIdAdmin(Guid id)
    //{
    //    var movie = await db.Movies.SingleOrDefaultAsync(c => c.Id == id);

    //    if (movie != null)
    //    {
    //        MovieViewModel movieModel = new MovieViewModel();
    //        movieModel.Name = movie.Name;
    //        movieModel.Description = movie.Description;
    //        movieModel.StartDate = movie.StartDate;
    //        movieModel.EndDate = movie.EndDate;
    //        movieModel.Price = movie.Price;
    //        movieModel.Rate = (int)movie.Rate;

    //        return movieModel;
    //    }

    //    // Trả về null nếu không tìm thấy movie
    //    return null;
    //}
    public MovieViewModel GetMovieByIdAdmin(Guid id)
    {

        var movie = db.Movies.SingleOrDefault(c => c.Id == id);
        if (movie != null)
        {


            MovieViewModel movieModel = new MovieViewModel();

            movieModel.Name = movie.Name;
            movieModel.Description = movie.Description;
            movieModel.StartDate = movie.StartDate;
            movieModel.EndDate = movie.EndDate;
            movieModel.Price = movie.Price;
            movieModel.Rate = (int)movie.Rate;



            return movieModel;
        }
        return null;
    }
    public async Task Update(MovieViewModel editMovie, Guid Mid, IFormFile Image)
    {
        var movie = db.Movies.SingleOrDefault(c => c.Id == Mid);
        if (movie != null)
        {




            movie.Name = editMovie.Name;
            movie.Id = Mid;
            movie.Description = editMovie.Description;
            movie.StartDate = editMovie.StartDate;
            movie.EndDate = editMovie.EndDate;
            movie.Price = editMovie.Price;
            if (Image != null)
            {
                movie.Image = editMovie.Image;
            }
            movie.Rate = editMovie.Rate;
            movie.Cat_Id = editMovie.Category_Id;
            movie.Producer_Id = editMovie.Producer_Id;

            if (editMovie.ActorIds != null)
            {
                foreach (var id in editMovie.ActorIds)
                {
                    db.MovieActors.Update(new MovieActor()
                    {
                        MovieId = Mid,
                        ActorId = id
                    });
                }
            }
            //adding to cinema movies table
            if (editMovie.CinemaIds != null)
                foreach (var id in editMovie.CinemaIds)
                {
                    db.MovieInCinemas.Add(new MoviesInCinema()
                    {
                        MovieId = Mid,
                        CinemaId = id
                    });
                }
        }
        db.Movies.Update(movie);
        await db.SaveChangesAsync();

    }
       
    private async Task<string> SaveImage(IFormFile image)
    {
        var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay  đổi đường dẫn theo cấu hình của bạn 
        using (var fileStream = new FileStream(savePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return "/images/" + image.FileName; // Trả về đường dẫn tương đối }

    }
    public async Task<Movie> GetByNameAsync(string name)
    {
        return await db.Movies.FindAsync(name);
    }
    

    public async Task InsertAsync(Movie movie)
    {
        db.Movies.Add(movie);
        await db.SaveChangesAsync();

    }

    public async Task UpdateAsync(Movie editMovie)
    {
        db.Movies.Update(editMovie);
        await db.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var movie = await db.Movies.FindAsync(id);
        db.Movies.Remove(movie);
        await db.SaveChangesAsync();
    }
    public async Task<IEnumerable<Movie>> GetProductByCategoryAsync(int id)
    {
        var list = db.Movies.Where(p => p.Cat_Id == id).ToList();
        return list;
    }
    public async Task<IEnumerable<Movie>> SearchAsync(string keyword)
    {
        // Thực hiện tìm kiếm sản phẩm trong cơ sở dữ liệu
        var searchResults = await db.Movies
            .Where(p => p.Name.Contains(keyword)) // Tìm sản phẩm có tên chứa từ khóa
            .ToListAsync();

        return searchResults;
    }

}