namespace loteriaBack.Models
{
    public class ResultadoSorteoSemanal
    {
        public int Semana { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public ResultadoLoteria? Bogota { get; set; }
        public ResultadoLoteria? Boyaca { get; set; }
        public ResultadoLoteria? Cauca { get; set; }
        public ResultadoLoteria? CruzRoja { get; set; }
        public ResultadoLoteria? Cundinamarca { get; set; }
        public ResultadoLoteria? Huila { get; set; }
        public ResultadoLoteria? Manizales { get; set; }
        public ResultadoLoteria? Medellin { get; set; }
        public ResultadoLoteria? Meta { get; set; }
        public ResultadoLoteria? Quindio { get; set; }
        public ResultadoLoteria? Risaralda { get; set; }
        public ResultadoLoteria? Santander { get; set; }
        public ResultadoLoteria? Tolima { get; set; }
        public ResultadoLoteria? Valle { get; set; }
        
    }
}
