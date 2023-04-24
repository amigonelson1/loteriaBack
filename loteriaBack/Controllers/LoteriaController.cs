using loteriaBack.Models;
using loteriaBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace loteriaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteriaController : ControllerBase
    {
        private int seed;

        [HttpGet]
        [Route("GetSorteos/{numeroSorteos}/{nombreLoteria}/{fecha}")]
        public async Task<IActionResult> GetSorteos(int numeroSorteos, string nombreLoteria ,DateTime fecha)
        {
            try
            {
                if (numeroSorteos == 0) { numeroSorteos = 5; }
                if (fecha.Year == 1) { fecha = DateTime.Now; }
                List<ResultadoLoteria> sorteos = new();
                int[] listaIndividuales = new int[4];

                for (int i = 0; i < numeroSorteos; i++)
                {
                    ResultadoLoteria sorteo = new(nombreLoteria,fecha.AddDays(-7*i));
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
                            //Console.WriteLine($"El caracter '{grupo.Key}' se repite {grupo.Count()} veces en la cadena.");
                            if (grupo.Count() == 2) { sorteo.DosCifras = true; }
                            if (grupo.Count() == 3) { sorteo.TresCifras = true; }
                            if (grupo.Count() == 4) { sorteo.CuatroCifras = true; }
                        }
                    }

                    sorteos.Add(sorteo);
                }

                return Ok(sorteos);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        
    }
}
