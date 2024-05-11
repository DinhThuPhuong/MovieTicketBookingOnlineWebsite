namespace DoAnCoSoTL.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string TimeSlot { get; set; }
        public string CinemaName { get; set; }
        public string CinemaLocation { get; set; }
        public string ListSeat { get; set; }
        public int SeatId {  get; set; }
        public Order Order { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema {  get; set; }
        public Seat Seat { get; set; }
    }
}
