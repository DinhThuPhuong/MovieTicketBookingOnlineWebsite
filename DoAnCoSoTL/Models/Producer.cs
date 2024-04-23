using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSoTL.Models
{
    public class Producer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string Bio { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
