using ApiTest.DTOs.Genres;
using ApiTest.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [ProducesResponseType<ApiResponse<List<GenreDto>>>(StatusCodes.Status200OK)]
        [EndpointSummary("Listar todos los géneros de películas")]
        [EndpointDescription("Obtiene una lista de todos los géneros de películas.")]
        [HttpGet]
        public async Task<ActionResult> GetAllGenres([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _genreService.GetAllGenres(currentPage, pageSize);
            return Ok(response);
        }

        [ProducesResponseType<ApiResponse<GenreDto>>(StatusCodes.Status200OK)]
        [EndpointSummary("Listar un género por ID")]
        [EndpointDescription("Obtiene un género de película por su ID.")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetGenreById(int id)
        {
            var genre = await _genreService.GetGenreById(id);
            return Ok(genre);
        }

        // POST: api/Genres
        [EndpointSummary("Insertar un nuevo género de película")]
        [EndpointDescription("Crea un nuevo género de película en la base de datos.")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GenreDto>>> CreateGenre(GenreCreateDto genre)
        {
            var newGenre = await _genreService.CreateGenre(genre);
            return Ok(newGenre);
        }

        // PUT: api/Genres/5
        [EndpointSummary("Actualizar un género de película por ID")]
        [EndpointDescription("Actualiza un género de película en la base de datos.")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GenreDto>>> UpdateGenre(int id, GenreCreateDto genre)
        {
            var updatedGenre = await _genreService.UpdateGenre(id, genre);
            return Ok(updatedGenre);
        }

        // DELETE: api/Genres/5
        [EndpointSummary("Eliminar un género de película por ID")]
        [EndpointDescription("Elimina un género de película de la base de datos.")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            var deleteGenre = await _genreService.DeleteGenre(id);
            return Ok(deleteGenre);
        }
    }
}
