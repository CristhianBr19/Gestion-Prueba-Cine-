using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracFullStack.Contexts;
using PracFullStack.Models;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class peliculasController : ControllerBase
    {
        private readonly MoviesContext _context;

        public peliculasController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pelicula>>> GetPeliculas()
        {
            return await _context.peliculas.OrderBy(p=>p.id).ToListAsync();
        }

        // GET: api/peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pelicula>>GetPeliculaId(int id)
        {
            var ids =  await _context.peliculas.FirstOrDefaultAsync(p => p.id == id);
            if(ids == null)
            {
                return NotFound();
            }
            return Ok(ids);
        }

        // PUT: api/peliculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task <IActionResult>PostPelicul(int id, pelicula peli)
        {
            if(id != peli.id)
            {
                return BadRequest();

            }
            _context.Entry(peli).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!peliculaExists(id))
                {
                    return NotFound("Pelicula no encontrada");
                }
                else
                {
                    throw;
                }    

               
            }
            return NoContent();

        }


        // POST: api/peliculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<pelicula>> PostPelicula([FromBody] pelicula peli)
        {
            if(peli == null)
            {
                return BadRequest();
               
            }else if(peli.fecha_publicacion == null)
            {
                return BadRequest("Fecha es requerido");
            }
            await _context.peliculas.AddAsync(peli);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPeliculaId), new { id = peli.id }, peli);
        }

        // DELETE: api/peliculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeletePelicula(int id)
        {
            var peli = await _context.peliculas.FindAsync(id);
            if (peli == null)
            {
                return NotFound(); 

            }

            _context.peliculas.Remove(peli);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<pelicula>> GetSearch(string? search)

        {
            var peliculanom = _context.peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                peliculanom = peliculanom.Where(p => p.nombre.ToLower().Contains(search));
            }

            var resultadoq = await peliculanom.ToListAsync();

            if (!resultadoq.Any())
            {
                return NotFound("No se encontro la pelicula");


            }
            return Ok(resultadoq);
        }

        [HttpGet("searchFecha/{fecha}")]

        public async Task<ActionResult<pelicula>>GetPeliculaByFecha(DateTime fecha)
        {
            var peliculas = await _context.peliculas.Where(p => p.fecha_publicacion.Value.Date == fecha.Date).ToListAsync();
            if(peliculas == null || !peliculas.Any())
            {
                return NotFound("No hay peliculas con esta fecha");

            }
            return Ok( peliculas);
        }

        [HttpPut("desactivate/{id}")]

        public async Task<IActionResult>DesactivatePelicula(int id)
        {
            var peliculaDesac = await _context.peliculas.FindAsync(id);
            if(peliculaDesac == null)
            {
                return NotFound("Pelicula no encontrada ");
            }

            peliculaDesac.active = false;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(505, "Error al actualizar la pelicula "); 
            }

            return NoContent();
        }

                                                                                    


        private bool peliculaExists(int id)
        {
            return _context.peliculas.Any(e => e.id == id);
        }
    }
}
