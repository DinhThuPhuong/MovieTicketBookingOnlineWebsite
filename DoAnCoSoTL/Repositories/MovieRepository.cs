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