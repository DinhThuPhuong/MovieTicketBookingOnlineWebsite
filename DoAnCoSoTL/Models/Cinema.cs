using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSoTL.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }
        public virtual List<MovieInCinema> MoviesInCinema { get; set; }
        public virtual ICollection<MovieShowtime> MovieShowtimes { get; set; }
    }
}
