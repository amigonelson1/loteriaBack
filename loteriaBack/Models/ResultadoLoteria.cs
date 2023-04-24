using System.ComponentModel.DataAnnotations;

namespace loteriaBack.Models
{
    public class ResultadoLoteria
    {
        public string NombreLoteria { get; set; }= string.Empty;
        public DateTime Fecha { get; set; }
        public int PrimeraCifra { get; set; }
        public int SegundaCifra { get; set; }
        public int TerceraCifra { get; set; }
        public int CuartaCifra { get; set; }
        public string Pleno { get; set; } = string.Empty;
        public string Ordenado { get; set; } = string.Empty;
        public bool DosCifras { get; set; }
        public bool TresCifras { get; set; }
        public bool CuatroCifras { get; set; }

       public ResultadoLoteria(string nombreLoteria, DateTime fecha)
        {
            NombreLoteria = nombreLoteria;
            Fecha = fecha;
        }        
        public ResultadoLoteria()
        {
               
        }

    }
}
