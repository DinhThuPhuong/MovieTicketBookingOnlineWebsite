using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [InverseProperty("Cinema")]
        public ICollection<Screening> Screenings { get; set; }
    }
}
