using ApiPeliculas.Controllers.Servicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracFullStack.Contexts;
using PracFullStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class peliculasController : ControllerBase
    {
        private readonly MoviesContext _context;
        private readonly PeliculaService _service;

        public peliculasController(MoviesContext context, PeliculaService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pelicula>>> GetPeliculas()
        {
            var peliculas = await _service.ObtenerTodo();
            return Ok(peliculas);
        }

        // GET: api/peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pelicula>> GetPeliculaId(int id)
        {
            var peli = await _service.ObtenerPorId(id);
            return peli == null ? NotFound() : Ok(peli);
        }

        [HttpPost]
        public async Task<ActionResult<pelicula>> PostPelicula([FromBody] pelicula peli)
        {
            if (peli?.fecha_publicacion == null) return BadRequest("Fecha es requerida"); // Validar fecha [cite: 31]
            await _service.Crear(peli);
            return CreatedAtAction(nameof(GetPeliculaId), new { id = peli.id }, peli);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, pelicula peli)
        {
            if (id != peli.id) return BadRequest();
            try { await _service.Actualizar(peli); }
            catch (DbUpdateConcurrencyException) { if (!_service.Existe(id)) return NotFound(); throw; }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            if (await _service.ObtenerPorId(id) == null) return NotFound();
            await _service.EliminarFisico(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<pelicula>>> GetSearch(string? search)
        {
            var res = await _service.BuscarNombre(search ?? "");
            return !res.Any() ? NotFound("No se encontró la película") : Ok(res); // Buscar por nombre [cite: 29]
        }

        [HttpGet("searchFecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<pelicula>>> GetPeliculaByFecha(DateTime fecha)
        {
            var res = await _service.BuscarFecha(fecha);
            return !res.Any() ? NotFound("No hay películas con esta fecha") : Ok(res); // Presentar por fecha [cite: 30]
        }

        [HttpPut("desactivate/{id}")]
        public async Task<IActionResult> DesactivatePelicula(int id)
        {
            if (await _service.ObtenerPorId(id) == null) return NotFound();
            await _service.Desactivar(id); // Eliminación lógica [cite: 38]
            return NoContent();
        }
    }
}