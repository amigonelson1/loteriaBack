using loteriaBack.Models;
using loteriaBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace loteriaBack.Controllers
{

//método get para generar y devolver los resultados de una unica lotería desde ciera fecha inicial hasta la fecha presente;
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

//método get para obtener el listado de resultados diarios de todas las loterias Colombianas en rangos de fechas establecidos;

        [HttpGet]
        [Route("GetSemanal/{fechaInicial}/{fechaFinal}")]
        public async Task<IActionResult> GetSorteoSemanal(DateTime fechaInicial,DateTime fechaFinal )
        {
            try
            {
                if (fechaFinal <= fechaInicial) { return BadRequest("Fechas en orden o valores invalidos"); }// validamos que fecha final no sea igual ni menor a la fecha inicial;
                List<ResultadoSorteoSemanal> semanales = new(); // nuevo listados de tipo ResultadoSorteoSemanal
                string periodo = string.Empty; // inicializamos el periodo
                while (fechaFinal> fechaInicial) // hacemos un ciclo condicionado por las fechas para ingresar los datos a nuestro listado de semanales; 
                {
                    ResultadoSorteoSemanal semanal = new(); // creamos una nueva clase semanal de tipo ResultadoSorteoSemanal;
                    semanal.Semana = ISOWeek.GetWeekOfYear(fechaInicial);   //agregamos el numero de semana;                
                    semanal.Periodo = fechaInicial.ToString("dd/MM/yyyy") + " - " + fechaInicial.AddDays(6).ToString("dd/MM/yyyy"); // agregamos el periodo
                    for(int i=0; i<7; i++) // ciclo para recorrer toda la semana y asignar a su respectiva loteria la fecha;
                    {
                        var dia = fechaInicial.AddDays(i); //fecha irá en aumento a medida avanza el ciclo for
                        switch (Convert.ToInt32(dia.DayOfWeek))
                        {
                            case 0:
                                { };
                                break;
                            case 1:
                                { semanal.Cundinamarca = GetSorteoPorLoteria(1, "Cundinamarca", dia.AddDays(7)); };
                                { semanal.Tolima = GetSorteoPorLoteria(1, "Tolima", dia.AddDays(7)); };                                
                                break;
                            case 2:
                                { semanal.CruzRoja = GetSorteoPorLoteria(1, "Cruz Roja", dia.AddDays(7)); };
                                { semanal.Huila = GetSorteoPorLoteria(1, "Huila", dia.AddDays(7)); };
                                break;
                            case 3:
                                { semanal.Manizales = GetSorteoPorLoteria(1, "Manizales", dia.AddDays(7)); };
                                { semanal.Meta = GetSorteoPorLoteria(1, "Meta", dia.AddDays(7)); };
                                { semanal.Valle = GetSorteoPorLoteria(1, "Valle", dia.AddDays(7)); };
                                break;
                            case 4:
                                { semanal.Bogota = GetSorteoPorLoteria(1, "Bogotá", dia.AddDays(7)); };
                                { semanal.Quindio = GetSorteoPorLoteria(1, "Quindío", dia.AddDays(7)); };
                                break;
                            case 5:                                
                                { semanal.Medellin = GetSorteoPorLoteria(1, "Medellín", dia.AddDays(7)); };
                                { semanal.Risaralda = GetSorteoPorLoteria(1, "Risaralda", dia.AddDays(7)); };
                                { semanal.Santander = GetSorteoPorLoteria(1, "Santander", dia.AddDays(7)); };
                                break;
                            case 6:
                                { semanal.Boyaca = GetSorteoPorLoteria(1, "Boyacá", dia.AddDays(7)); };
                                { semanal.Cauca = GetSorteoPorLoteria(1, "Cauca", dia.AddDays(7)); };
                                break;

                        }
                    }
                    fechaInicial = fechaInicial.AddDays(7);// pasado el ciclo agregamos una semana mas a nuestra fecha;
                    semanales.Add(semanal);
                }
                return Ok(semanales);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


// metodo get para llamar, elaborar y devolver el método de la columna con cuatro y tres cifras de manera plena y ordenada;

        [HttpGet]
        [Route("GetMetodoColumna/{pleno}")]
        public async Task<IActionResult> GetMetodoColumna(string pleno)
        {
            try
            {
                List<ResultadoMetodoColumna> resultadosMetodoColumna = new();
                string numero4 = pleno;
                for (int i = 0; i < 9; i++)
                {
                    ResultadoMetodoColumna resultadoMetodoColumna = new();                    
                    char[] digitos = numero4.ToCharArray(); // Convertir la cadena a un arreglo de caracteres                   
                    string resultado = ""; // Variable para almacenar el resultado
                                           // Recorrer el arreglo de caracteres
                    foreach (char digito in digitos)
                    {
                        int valor = int.Parse(digito.ToString()); // Convertir el carácter a un número entero
                        valor++; // Sumar uno al valor
                        if (valor == 10) { valor = 0; }
                        resultado += valor.ToString(); // Agregar el valor al resultado como cadena de texto
                    }
                    numero4 = resultado;
                    resultadoMetodoColumna.Generado4Cifras = numero4;
                    char[] chars = resultadoMetodoColumna.Generado4Cifras.ToCharArray(); //separamos nuestro numero string para poderlo manipular
                    Array.Sort(chars); //organizamos de menor a mayor el numero;
                    resultadoMetodoColumna.OrdenadoGenerado4Cifras = new string(chars);                    
                    resultadoMetodoColumna.Generado3Cifras = numero4.Substring(1, 3);
                    char[] chars3 = resultadoMetodoColumna.Generado3Cifras.ToCharArray(); 
                    Array.Sort(chars3);
                    resultadoMetodoColumna.OrdenadoGenerado3Cifras = new string(chars3);
                    resultadosMetodoColumna.Add(resultadoMetodoColumna);
                }

                return Ok(resultadosMetodoColumna);

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

//función para crear el listado de tipo ResultadoLoteria

        private static List<ResultadoLoteria> GetSorteosPorLoteria(int numeroSorteos, string nombreLoteria, DateTime fecha)
        {
            if (numeroSorteos == 0) { numeroSorteos = 5; } // para generar numero de sorteos y fechas por defecto en caso de venir vacio esos campos;
            if (fecha.Year == 1) { fecha = DateTime.Now; }
            List<ResultadoLoteria> sorteos = new();
            for (int i = 0; i < numeroSorteos; i++) //ciclo condicionado por la cantidad de sorteos que sean solicitados;
            {
                var sorteo = GetSorteoPorLoteria(i, nombreLoteria, fecha);
                sorteos.Add(sorteo);
            }
            return sorteos;
        }

// función para crear el individual de cada uno de los ResultadoLoteria;

        private static ResultadoLoteria GetSorteoPorLoteria(int numeroSorteo, string nombreLoteria, DateTime fecha)
        {
            ResultadoLoteria sorteo = new(nombreLoteria, fecha.AddDays(-7 * numeroSorteo)); //Restamos una semana por cada ciclo que se va generando de la anterior funcion;
            int[] listaIndividuales = new int[4]; // array para guardar cada uno de los cuatro dígitos solicitados;
            for (int j = 0; j < 4; j++) // for para generar cada uno de los 4 aleatorios
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

            sorteo.Pleno = $"{sorteo.PrimeraCifra}{sorteo.SegundaCifra}{sorteo.TerceraCifra}{sorteo.CuartaCifra}"; // concatenamos en string los valores obtenidos por individual para tener nuestro numero pleno;
            sorteo.Pleno3Cifras = $"{sorteo.SegundaCifra}{sorteo.TerceraCifra}{sorteo.CuartaCifra}";
            char[] chars = sorteo.Pleno.ToCharArray(); //separamos nuestro numero string para poderlo manipular
            Array.Sort(chars); //organizamos de menor a mayor el numero;
            sorteo.Ordenado = new string(chars);
            char[] chars3 = sorteo.Pleno3Cifras.ToCharArray(); //separamos nuestro numero string para poderlo manipular
            Array.Sort(chars3); //organizamos de menor a mayor el numero;
            sorteo.Ordenado3Cifras = new string(chars3);
            var grupos = sorteo.Pleno.GroupBy(c => c); // agrupamos por cantidad de numeros iguales 
            foreach (var grupo in grupos)
            {
                if (grupo.Count() > 1)
                {
                    if (grupo.Count() == 2) { sorteo.DosCifras = true; }
                    if (grupo.Count() == 3) { sorteo.TresCifras = true; }
                    if (grupo.Count() == 4) { sorteo.CuatroCifras = true; }
                }
            }
            var grupos3 = sorteo.Pleno3Cifras.GroupBy(c => c); // agrupamos por cantidad de numeros iguales 
            foreach (var grupo in grupos3)
            {
                if (grupo.Count() > 1)
                {
                    if (grupo.Count() == 2) { sorteo.DosCifras3 = true; }
                    if (grupo.Count() == 3) { sorteo.TresCifras3 = true; }
                }
            }
            return sorteo;
        }

    }
}
