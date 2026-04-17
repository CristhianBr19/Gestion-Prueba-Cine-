using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PracFullStack.Contexts;
using PracFullStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sala_cineController : ControllerBase
    {
        private readonly MoviesContext _context;

        public sala_cineController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/sala_cine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<sala_cine>>> Getsala_cines()
        {
            return await _context.sala_cines.OrderBy(s=>s.id).ToListAsync();
        }

        // GET: api/sala_cine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<sala_cine>> Getsala_cine(int id)
        {
            var sala_cine = await _context.sala_cines.FindAsync(id);

            if (sala_cine == null)
            {
                return NotFound();
            }

            return sala_cine;
        }

        // PUT: api/sala_cine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putsala_cine(int id, sala_cine sala_cine)
        {
            if (id != sala_cine.id)
            {
                return BadRequest();
            }

            _context.Entry(sala_cine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sala_cineExists(id))
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

        // POST: api/sala_cine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<sala_cine>> Postsala_cine(sala_cine sala_cine)
        {
            _context.sala_cines.Add(sala_cine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getsala_cine", new { id = sala_cine.id }, sala_cine);
        }

        // DELETE: api/sala_cine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletesala_cine(int id)
        {
            var sala_cine = await _context.sala_cines.FindAsync(id);
            if (sala_cine == null)
            {
                return NotFound();
            }

            _context.sala_cines.Remove(sala_cine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("disponibilidad/{nombre_sala}")]
        public async Task<ActionResult<string>> GetDisponible(string nombre_sala)
        {
            var conexion = _context.Database.GetDbConnection();
            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                {
                    await conexion.OpenAsync();
                }

                
                using (var comando = conexion.CreateCommand())
                {
               
                    var parametro = new NpgsqlParameter("@p0", nombre_sala);
                  
                    comando.Parameters.Add(parametro);
                    comando.CommandText = "SELECT public.fn_consultar_disponibilidad_sala(@p0)";

                    
                    var resultado = await comando.ExecuteScalarAsync();
                    return Ok(resultado?.ToString() ?? "sin datos");

                }
             
            }
            finally
            {

                if (conexion.State == System.Data.ConnectionState.Open)
                {

                    await conexion.CloseAsync();

                }

            }
        }





        [HttpGet("search")]
        public async Task<ActionResult<sala_cine>>SearchSala(string? search)
        {

            var salas = _context.sala_cines.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                salas = salas.Where(s => s.nombre.ToLower().Contains(search));
            }
            var resultado = await salas.ToListAsync();

            if (!resultado.Any())
            {
                return NotFound("No se encontro la sala");
            }

            foreach(var sal in resultado)
            {
                
                var disponibilidad = await _context.Database
                    .SqlQueryRaw<string>("SELECT public.fn_consultar_disponibilidad_sala({0})", sal.nombre)
                    .ToListAsync();

                sal.estadoReal = disponibilidad.FirstOrDefault();
            
        }

            return Ok(resultado);

        }

        [HttpPut ("desactivate/{id}")]
        public async Task<IActionResult>DesactivateSala(int id)
        {
            var saladesact = await _context.sala_cines.FindAsync(id);
            if(saladesact == null)
            {
                return NotFound("Sala no encontrada"); 

            }

            saladesact.active = false;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return StatusCode(505, "Error al actualizar la sala");
            }
            return NoContent();
        }



        private bool sala_cineExists(int id)
        {
            return _context.sala_cines.Any(e => e.id == id);
        }
    }
}
