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
    public async Task<Movie> GetByIdAsync(int id)
    {
        return await db.Movies.FindAsync(id);
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

    public async Task InsertAsync(MovieViewModel movievm, IFormFile Image)
    {
        if (Image != null)
        {
            // Lưu hình ảnh đại diện
            movievm.Image = await SaveImage(Image);
        }

        var movie = new Movie()
        {
            Name = movievm.Name,
            StartDate = movievm.StartDate,
            EndDate = movievm.EndDate,
            Price = movievm.Price,
            Description = movievm.Description,
            Cat_Id = movievm.Category_Id,
            Rate = movievm.Rate,
            Producer_Id = movievm.Producer_Id,
            Image = movievm.Image,
            Trailer = movievm.Trailer
        };

        db.Movies.Add(movie);

        // Thêm các bản ghi liên quan vào cơ sở dữ liệu
        foreach (var id in movievm.ActorIds)
        {
            db.MovieActors.Add(new MovieActor()
            {
                Movie = movie,
                ActorId = id
            });
        }

        for (var i = 0; i < movievm.CinemaIds.Count; i++)
        {
            db.MovieInCinemas.Add(new MovieInCinema()
            {
                Quantity = movievm.Quantities[i],
                Movie = movie,
                CinemaId = movievm.CinemaIds[i]
            });
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        await db.SaveChangesAsync();
    }


    public async Task UpdateAsync(Movie editMovie)
    {
        db.Movies.Update(editMovie);
        await db.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var movie = await db.Movies.FindAsync(id);
        db.Movies.Remove(movie);
        await db.SaveChangesAsync();
    }

}