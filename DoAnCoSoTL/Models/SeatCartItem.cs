﻿namespace DoAnCoSoTL.Models
{
    public class SeatCartItem
    {
        public int SeatId { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public string CinemaLocation { get; set; }
        public string TimeSlot { get; set; }
        public decimal Price { get; set; }
    }
}
