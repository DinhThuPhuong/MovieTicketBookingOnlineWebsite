using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSoTL.Models
{
    public class Seat
    {
        public int Id { get; set; }

        [Required]
        public string SeatCode { get; set; } // Mã ghế kiểu string

        [Required]
        public string Row { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [ForeignKey("Cinema")]
        public int CinemaId { get; set; }

        [ForeignKey("Screening")]
        public int ScreeningId { get; set; }

        public virtual Cinema Cinema { get; set; }

        public virtual Screening Screening { get; set; }

        // Thêm trường để xác định trạng thái của ghế (đã đặt hay chưa)
        public bool IsBooked { get; set; }
    }
}
