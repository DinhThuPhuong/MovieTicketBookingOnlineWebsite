//using System.ComponentModel.DataAnnotations.Schema;

//namespace DoAnCoSoTL.Models
//{
//    public class ChooseSeatsViewModel
//    {
//        public List<string> BookedSeats { get; set; } // Danh sách các ghế đã đặt
//        [ForeignKey("Cinema")]
//        public int CinemaId { get; set; }

//        [ForeignKey("Screening")]
//        public int ScreeningId { get; set; }

//        public virtual Cinema Cinema { get; set; }
//        public virtual Screening Screening { get; set; }

//        public ChooseSeatsViewModel()
//        {
//            // Khởi tạo danh sách ghế đã đặt
//            BookedSeats = new List<string>();
//        }
//    }
//}
