using System.ComponentModel.DataAnnotations;

namespace loteriaBack.ViewModels
{
    public class Loteria
    {
        [Required]
        public int numeroSorteos { get; set; }
        [Required]
        public string nombreLoteria { get; set; }
        [Required]
        public DateTime fecha { get; set; }
    }
}
