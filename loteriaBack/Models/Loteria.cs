using System.ComponentModel.DataAnnotations;

namespace loteriaBack.Models
{
    public class Loteria
    {
        [Required]
        public int sorteos { get; set; }
        [Required]
        public string loteria { get; set; }
        [Required]
        public DateTime fecha { get; set; }
    }
}
