using ApiPeliculas.Repository;
using PracFullStack.Models;

namespace ApiPeliculas.Controllers.Servicies
{
    public class PeliculaService

    {
        private readonly PeliculaRepository _repo;
        public PeliculaService(PeliculaRepository repo) => _repo = repo;

        public async Task<List<pelicula>> ObtenerTodo() => await _repo.GetAllAsync();

        public async Task<pelicula> ObtenerPorId(int id) => await _repo.GetByIdAsync(id);

        public async Task Crear(pelicula peli) => await _repo.AddAsync(peli);

        public async Task Actualizar(pelicula peli) => await _repo.UpdateAsync(peli);

        public async Task EliminarFisico(int id) => await _repo.DeletePhysicalAsync(id);

        public async Task<List<pelicula>> BuscarNombre(string nombre) => await _repo.SearchByNameAsync(nombre);

        public async Task<List<pelicula>> BuscarFecha(DateTime fecha) => await _repo.SearchByDateAsync(fecha);

        public async Task Desactivar(int id)
        {
            var peli = await _repo.GetByIdAsync(id);
            if (peli != null)
            {
                peli.active = false; 
                await _repo.UpdateAsync(peli);
            }
        }

        public bool Existe(int id) => _repo.Exists(id);
    }
}


