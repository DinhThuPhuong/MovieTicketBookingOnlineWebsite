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
            movievm.Image = await SaveImage(Image); // Phương thức xử lý hình ảnh đã được tách ra
        }
        var newGuid = Guid.NewGuid();
        // Kiểm tra tính hợp lệ của dữ liệu đầu vào
        var movie = new Movie()
        {
            Id = newGuid,
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

        // Thêm liên kết với diễn viên vào cơ sở dữ liệu
        // Tạo một Dictionary để lưu trữ cặp key-value (cinemaId, quantity)
        //Adding to actor movies table
        foreach (var id in movievm.ActorIds)
        {
            db.MovieActors.Add(new MovieActor()
            {
                MovieId = newGuid,
                ActorId = id
            });
        }
        //adding to cinema movies table
        for (var i = 0; i < movievm.CinemaIds.Count; i++)
        {
            db.MovieInCinemas.Add(new MovieInCinema()
            {
                Quantity = movievm.Quantities[i],
                MovieId = newGuid,
                CinemaId = movievm.CinemaIds[i]
            });
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        await db.SaveChangesAsync();
    }


    //    public async Task InsertAsync(MovieViewModel movievm, IFormFile Image)
    //{
    //    if (Image != null)
    //    {
    //        movievm.Image = await SaveImage(Image); // Phương thức xử lý hình ảnh đã được tách ra
    //    }

    //    // Kiểm tra tính hợp lệ của dữ liệu đầu vào
    //        var movie = new Movie()
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = movievm.Name,
    //            StartDate = movievm.StartDate,
    //            EndDate = movievm.EndDate,
    //            Price = movievm.Price,
    //            Description = movievm.Description,
    //            Cat_Id = movievm.Category_Id,
    //            Rate = movievm.Rate,
    //            Producer_Id = movievm.Producer_Id,
    //            Image = movievm.Image,
    //            Trailer = movievm.Trailer
    //        };

    //        db.Movies.Add(movie);

    //        // Thêm liên kết với diễn viên và rạp chiếu vào cơ sở dữ liệu
    //        foreach (var id in movievm.ActorIds)
    //        {
    //            db.MovieActors.Add(new MovieActor()
    //            {
    //                MovieId = movie.Id,
    //                ActorId = id
    //            });
    //        }

    //        foreach (var cinemaId in movievm.CinemaIds)
    //        {
    //            db.MovieInCinemas.Add(new MovieInCinema()
    //            {
    //                Quantity = movievm.Quantities[movievm.CinemaIds.IndexOf(cinemaId)],
    //                MovieId = movie.Id,
    //                CinemaId = cinemaId
    //            });
    //        }

    //        // Lưu các thay đổi vào cơ sở dữ liệu
    //        await db.SaveChangesAsync();
    //}

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