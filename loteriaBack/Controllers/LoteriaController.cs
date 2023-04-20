using loteriaBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loteriaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteriaController : ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
               return Ok("solicitud exitosa!!!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // POST api/<TarjeaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Loteria loteria)
        {
            try
            {
                return Ok(loteria);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
