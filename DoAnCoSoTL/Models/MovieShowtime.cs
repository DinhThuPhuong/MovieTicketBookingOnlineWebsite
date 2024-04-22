using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSoTL.Models
{
        public class MovieShowtime
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("Movie")]
            public Guid MovieId { get; set; }
            public virtual Movie Movie { get; set; }

            [ForeignKey("Cinema")]
            public int CinemaId { get; set; }
            public virtual Cinema Cinema { get; set; }

            public DateTime Showtime { get; set; }

            public decimal TicketPrice { get; set; }

            public int AvailableTickets { get; set; }
        }
    }
