﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSoTL.Models
{
    public class MovieOrder
    {
        public int Id { get; set; }
        public int  Quantity { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        //public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
