using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSoTL.Models
{
    public class SeatReservation
    {
        public int Id { get; set; }

        [ForeignKey("Screening")]
        public int ScreeningId { get; set; }
        public virtual Screening Screening { get; set; }

        [ForeignKey("Seat")]
        public int SeatId { get; set; }
        public virtual Seat Seat { get; set; }

        [ForeignKey("ApplicationUser")] 
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public double TotalPrice { get; set; }
    }
}
