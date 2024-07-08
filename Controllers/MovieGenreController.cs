using ApiTest.DTOs.Genres;
using ApiTest.DTOs.Movies;
using ApiTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Movies by genre")]
    [Produces("application/json")]
    public class MovieGenreController : ControllerBase
    {
        private readonly MovieGenreService _movieGenreService;

        public MovieGenreController(MovieGenreService movieGenreService)
        {
            _movieGenreService = movieGenreService;
        }

        [ProducesResponseType<ApiResponse<List<MovieByGenreResponseDto>>>(StatusCodes.Status200OK)]
        [EndpointSummary("Listar todas las películas por id del género")]
        [EndpointDescription("Obtiene todas las películas por género.")]
        [HttpGet("{genreId}")]
        public async Task<ActionResult> GetMoviesByGenre(int genreId, [FromQuery] int currentPage = 1, [FromQuery] int pageSize = 10)
        {
            var movies = await _movieGenreService.GetMoviesByGenre(genreId, currentPage, pageSize);
            return Ok(movies);
        }
    }
}
