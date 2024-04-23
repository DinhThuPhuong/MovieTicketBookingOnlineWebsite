using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using DoAnCoSoTL.ViewModels;
using Microsoft.EntityFrameworkCore;

public class ScreeningRepository : IScreeningRepository
{
    MovieContext db;
    public ScreeningRepository(MovieContext _db)
    {
        db = _db;
    }
    public async Task InsertScreeningAsync(Screening screening)
    {
        db.Screenings.Add(screening);
        await db.SaveChangesAsync();
    }
    public IEnumerable<Cinema> GetSelectedCinemasByMovieId(Guid movieId)
    {
        return db.Screenings
                 .Where(s => s.MovieId == movieId)
                 .Select(s => s.Cinema)
                 .Distinct()
                 .ToList();
    }
    public async Task<IEnumerable<Screening>> GetAllAsync()
    {
        return await db.Screenings.ToListAsync();
    }
    public async Task<IEnumerable<Screening>> GetScreeningsByDate(DateTime date)
    {
        return db.Screenings.Where(s => s.Date == date.Date).ToList();
    }




}