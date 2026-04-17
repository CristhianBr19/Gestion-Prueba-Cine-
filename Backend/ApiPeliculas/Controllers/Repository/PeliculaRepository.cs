
using Microsoft.EntityFrameworkCore;
using PracFullStack.Contexts; 
using PracFullStack.Models;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository
    {
        private readonly MoviesContext _context;
        public PeliculaRepository(MoviesContext context) => _context = context;

        public async Task<List<pelicula>> GetAllAsync() => await _context.peliculas.Where(p => p.active == true).ToListAsync();

        public async Task<pelicula> GetByIdAsync(int id) => await _context.peliculas.FirstOrDefaultAsync(p => p.id == id);

        public async Task AddAsync(pelicula peli)
        {
            await _context.peliculas.AddAsync(peli);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(pelicula peli)
        {
            _context.Entry(peli).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhysicalAsync(int id)
        {
            var peli = await _context.peliculas.FindAsync(id);
            if (peli != null)
            {
                var relaciones = _context.pelicula_salacines.Where(p => p.id_pelicula == id);
                _context.pelicula_salacines.RemoveRange(relaciones);
                _context.peliculas.Remove(peli);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<pelicula>> SearchByNameAsync(string name) =>
            await _context.peliculas.Where(p => p.nombre.ToLower().Contains(name.ToLower()) && p.active == true).ToListAsync();

        public async Task<List<pelicula>> SearchByDateAsync(DateTime fecha) =>
            await _context.peliculas.Where(p => p.fecha_publicacion.Value.Date == fecha.Date).ToListAsync();

        public bool Exists(int id) => _context.peliculas.Any(e => e.id == id);
    }
}