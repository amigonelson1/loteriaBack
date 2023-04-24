using loteriaBack.Models;
using loteriaBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace loteriaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteriaController : ControllerBase
    {

        [HttpGet]
        [Route("GetSorteos/{numeroSorteos}/{nombreLoteria}/{fecha}")]
        public async Task<IActionResult> GetSorteos(int numeroSorteos, string nombreLoteria ,DateTime fecha)
        {
            try
            {
                var sorteos = GetSorteosPorLoteria(numeroSorteos, nombreLoteria, fecha);  
                return Ok(sorteos);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetSemanal/{fechaInicial}/{fechaFinal}")]
        public async Task<IActionResult> GetSorteoSemanal(DateTime fechaInicial,DateTime fechaFinal )
        {
            try
            {                
                List<ResultadoSorteoSemanal> semanales = new();
                string periodo = string.Empty;
                while (fechaFinal> fechaInicial)
                {
                    ResultadoSorteoSemanal semanal = new(); 
                    semanal.Semana = ISOWeek.GetWeekOfYear(fechaInicial);                    
                    semanal.Periodo = fechaInicial.ToString("dd/MM/yyyy") + " - " + fechaInicial.AddDays(6).ToString("dd/MM/yyyy");
                    for(int i=0; i<7; i++)
                    {
                        var dia = fechaInicial.AddDays(i);
                        switch (Convert.ToInt32(dia.DayOfWeek))
                        {
                            case 0:
                                { };
                                break;
                            case 1:
                                { semanal.Cundinamarca = GetSorteoPorLoteria(1, "Cundinamarca", dia); };
                                { semanal.Tolima = GetSorteoPorLoteria(1, "Tolima", dia); };                                
                                break;
                            case 2:
                                { semanal.CruzRoja = GetSorteoPorLoteria(1, "Cruz Roja", dia); };
                                { semanal.Huila = GetSorteoPorLoteria(1, "Huila", dia); };
                                break;
                            case 3:
                                { semanal.Manizales = GetSorteoPorLoteria(1, "Manizales", dia); };
                                { semanal.Meta = GetSorteoPorLoteria(1, "Meta", dia); };
                                { semanal.Valle = GetSorteoPorLoteria(1, "Valle", dia); };
                                break;
                            case 4:
                                { semanal.Bogota = GetSorteoPorLoteria(1, "Bogotá", dia); };
                                { semanal.Quindio = GetSorteoPorLoteria(1, "Quindío", dia); };
                                break;
                            case 5:                                
                                { semanal.Medellin = GetSorteoPorLoteria(1, "Medellín", dia); };
                                { semanal.Risaralda = GetSorteoPorLoteria(1, "Risaralda", dia); };
                                { semanal.Santander = GetSorteoPorLoteria(1, "Santander", dia); };
                                break;
                            case 6:
                                { semanal.Boyaca = GetSorteoPorLoteria(1, "Boyacá", dia); };
                                { semanal.Cauca = GetSorteoPorLoteria(1, "Cauca", dia); };
                                break;

                        }
                    }
                    fechaInicial = fechaInicial.AddDays(7);
                    semanales.Add(semanal);
                }
                return Ok(semanales);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        private static List<ResultadoLoteria> GetSorteosPorLoteria(int numeroSorteos, string nombreLoteria, DateTime fecha)
        {
            if (numeroSorteos == 0) { numeroSorteos = 5; }
            if (fecha.Year == 1) { fecha = DateTime.Now; }
            List<ResultadoLoteria> sorteos = new();
            for (int i = 0; i < numeroSorteos; i++)
            {
                var sorteo = GetSorteoPorLoteria(i, nombreLoteria, fecha);
                sorteos.Add(sorteo);
            }
            return sorteos;
        }

        private static ResultadoLoteria GetSorteoPorLoteria(int numeroSorteo, string nombreLoteria, DateTime fecha)
        {
            ResultadoLoteria sorteo = new(nombreLoteria, fecha.AddDays(-7 * numeroSorteo));
            int[] listaIndividuales = new int[4];
            for (int j = 0; j < 4; j++)
            {
                Random rnd = new();
                int randomNumber = rnd.Next(0, 10); //Genera un número aleatorio entre 0 y 9;
                listaIndividuales[j] = randomNumber;
                switch (j)
                {
                    case 0:
                        sorteo.PrimeraCifra = randomNumber;
                        break;
                    case 1:
                        sorteo.SegundaCifra = randomNumber;
                        break;
                    case 2:
                        sorteo.TerceraCifra = randomNumber;
                        break;
                    case 3:
                        sorteo.CuartaCifra = randomNumber;
                        break;
                }

            }

            sorteo.Pleno = $"{sorteo.PrimeraCifra}{sorteo.SegundaCifra}{sorteo.TerceraCifra}{sorteo.CuartaCifra}";
            char[] chars = sorteo.Pleno.ToCharArray();
            Array.Sort(chars);
            sorteo.Ordenado = new string(chars);
            var grupos = sorteo.Pleno.GroupBy(c => c);
            foreach (var grupo in grupos)
            {
                if (grupo.Count() > 1)
                {
                    if (grupo.Count() == 2) { sorteo.DosCifras = true; }
                    if (grupo.Count() == 3) { sorteo.TresCifras = true; }
                    if (grupo.Count() == 4) { sorteo.CuatroCifras = true; }
                }
            }
            return sorteo;
        }

    }
}
