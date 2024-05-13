using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DoAnCoSoTL.Models
{
    public class MovieContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        // Các DbSet khác
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MoviesInCinema> MovieInCinemas { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<MovieOrder> MovieOrders { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
            //InitializeSeats();
        }

        //private void InitializeSeats()
        //{
        //    if (!Seats.Any())
        //    {
        //        // Lặp qua tất cả các hàng và ghế trong rạp
        //        for (int row = 1; row <= 8; row++)
        //        {
        //            for (int seatNumber = 1; seatNumber <= 12; seatNumber++)
        //            {
        //                var seat = new Seat
        //                {
        //                    SeatCode = $"{(char)('A' + row - 1)}{seatNumber}",
        //                    Row = row.ToString(),
        //                    Number = seatNumber,
        //                    IsAvailable = true, // Ban đầu tất cả các ghế đều có sẵn
        //                    // Thay đổi hoặc bổ sung các trường khác tùy theo yêu cầu của bạn
        //                };

        //                Seats.Add(seat);
        //            }
        //        }

        //        SaveChanges();
        //    }
        //}
    }
}
