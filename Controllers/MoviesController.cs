using ApiTest.DTOs.Genres;
using ApiTest.DTOs.Movies;
using ApiTest.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [ProducesResponseType<ApiResponse<List<MovieResponseDto>>>(StatusCodes.Status200OK)]
        [EndpointSummary("Listar todas las películas")]
        [HttpGet]
        public async Task<ActionResult<List<ApiResponse<MovieResponseDto>>>> GetAllMovies([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
        {
            var movies = await _movieService.GetAllMovies(currentPage, pageSize);
            return Ok(movies);
        }

        [EndpointSummary("Listar una película por ID")]
        [EndpointDescription("Obtiene una película por su ID.")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MovieResponseDto>>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieById(id);
            return Ok(movie);
        }

        [EndpointSummary("Insertar una nueva película")]
        [EndpointDescription("Crea una nueva película en la base de datos.")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MovieResponseDto>>> CreateMovie(MovieCreateDto movie)
        {
            var newMovie = await _movieService.CreateMovie(movie);
            return Ok(newMovie);
        }

        [EndpointSummary("Actualizar una película por ID")]
        [EndpointDescription("Actualiza una película en la base de datos.")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MovieResponseDto>>> UpdateMovie(int id, MovieCreateDto movie)
        {
            var updatedMovie = await _movieService.UpdateMovie(id, movie);
            return Ok(updatedMovie);
        }


        [EndpointSummary("Eliminar una película por ID")]
        [EndpointDescription("Elimina una película de la base de datos.")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var deletedMovie = await _movieService.DeleteMovie(id);
            return Ok(deletedMovie);
        }
    }
}
