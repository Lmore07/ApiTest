using ApiTest.Classes;
using ApiTest.Context;
using ApiTest.DTOs.Genres;
using ApiTest.DTOs.Movies;
using ApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Services
{
    public class MovieGenreService
    {
        private readonly AppDBContext _context;

        public MovieGenreService(AppDBContext context)
        {
            _context = context;
        }

        // Get all movie by genre
        public async Task<ApiResponse<List<MovieByGenreResponseDto>>> GetMoviesByGenre(int genreId, int currentPage, int pageSize)
        {

            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.GenreId == genreId);

            if (genre == null)
            {
                throw new NotFoundException("Genre not found");
            }

            var movies = await _context.MovieGenre
               .Where(mg => mg.GenreId == genreId)
               .Where(mg => mg.Movie.Status == true)
               .Take(pageSize)
               .Skip((currentPage - 1) * pageSize)
               .Include(mg => mg.Movie)
               .ToListAsync();

            var totalCount = await _context.MovieGenre
                .Where(mg => mg.GenreId == genreId)
                .Where(mg => mg.Movie.Status == true)
                .CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginationInfo = new PaginationInfo(currentPage, totalPages, pageSize, totalCount);

            var movieResponseDto = new List<MovieByGenreResponseDto>();
            foreach (var movie in movies)
            {
                movieResponseDto.Add(MapMovieToMovieResponseDto(movie.Movie));
            }

            if (movieResponseDto.Count == 0)
            {
                throw new NotFoundException("Movies not found");
            }

            return new ApiResponse<List<MovieByGenreResponseDto>>(movieResponseDto, "Películas obtenidas con éxito.", paginationInfo);
        }

        private MovieByGenreResponseDto MapMovieToMovieResponseDto(Movies movie)
        {
            return new MovieByGenreResponseDto
            {
                MovieId = movie.MovieId,
                TitleMovie = movie.TitleMovie,
                ReleaseDate = movie.ReleaseDate
            };
        }
    }
}
