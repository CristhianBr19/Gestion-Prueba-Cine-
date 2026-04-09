using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Npgsql;
using PracFullStack.Contexts;
using PracFullStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pelicula_salacineController : ControllerBase
    {
        private readonly MoviesContext _context;

        public pelicula_salacineController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/pelicula_salacine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pelicula_salacine>>> Getpelicula_salacines()
        { 
            return await _context.pelicula_salacines.Include(p =>p.id_peliculaNavigation).Include(s =>s.id_salaNavigation).ToListAsync();
        }

        // GET: api/pelicula_salacine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pelicula_salacine>> Getpelicula_salacine(int id)
        {
            var pelicula_salacine = await _context.pelicula_salacines.FindAsync(id);

            if (pelicula_salacine == null)
            {
                return NotFound();
            }

            return pelicula_salacine;
        }

        // PUT: api/pelicula_salacine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpelicula_salacine(int id, pelicula_salacine pelicula_salacine)
        {
            if (id != pelicula_salacine.id)
            {
                return BadRequest();
            }

            _context.Entry(pelicula_salacine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pelicula_salacineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/pelicula_salacine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<pelicula_salacine>> Postpelicula_salacine(pelicula_salacine pelicula_salacine)
        {
            _context.pelicula_salacines.Add(pelicula_salacine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpelicula_salacine", new { id = pelicula_salacine.id }, pelicula_salacine);
        }

        // DELETE: api/pelicula_salacine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepelicula_salacine(int id)
        {
            var pelicula_salacine = await _context.pelicula_salacines.FindAsync(id);
            if (pelicula_salacine == null)
            {
                return NotFound();
            }

            _context.pelicula_salacines.Remove(pelicula_salacine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //recibir funcion sql 
        [HttpGet("disponibilidad /{nombre_sala}")]
        public async Task<ActionResult<string>>GetResultado(string nombre_sala)
        {
            var conexion = _context.Database.GetDbConnection();
            try
            {
                if(conexion.State != System.Data.ConnectionState.Open)
                {
                    await conexion.OpenAsync();
                }

                using (var comando = conexion.CreateCommand()) { 
                var parametro = new NpgsqlParameter("@p0", nombre_sala);
                    comando.Parameters.Add(parametro);

                    comando.CommandText= "SELECT public.fn_consultar_disponibilidad_sala(@p0)";
                    var resultado = await comando.ExecuteScalarAsync();

                    return Ok(resultado?.ToString() ?? "sin datos");


                }

            }
            finally
            {
                if(conexion.State == System.Data.ConnectionState.Open)
                {
                    await conexion.CloseAsync();
                }
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<pelicula_salacine>>GetSearch(string? search)
        {
            var peliSala = _context.pelicula_salacines.Include(p=>p.id_peliculaNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                peliSala = peliSala.Where(p => p.id_peliculaNavigation.nombre.ToLower().Contains(search.ToLower()));

               
            }

            var resultado = await peliSala.ToListAsync();

            if (!resultado.Any())
            {
                return NotFound("No se encontro la pelicula"); 
            }

            return Ok(resultado);
        }

        [HttpPut("desactive/{id}")]

        public async Task<IActionResult>Desactive(int id)
        {
            var peliSal = await _context.pelicula_salacines.FindAsync(id);

            if(peliSal == null)
            {
                return NotFound("Pelicula no encontrada");

            }

            peliSal.active = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return StatusCode(505, "Error al actualizar");

            }
            return NoContent();

        }

       
        private bool pelicula_salacineExists(int id)
        {
            return _context.pelicula_salacines.Any(e => e.id == id);
        }
    }
}
