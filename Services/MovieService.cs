using ApiTest.Classes;
using ApiTest.Context;
using ApiTest.DTOs.Genres;
using ApiTest.DTOs.Movies;
using ApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Services
{
    public class MovieService
    {
        private readonly AppDBContext _context;

        public MovieService(AppDBContext context)
        {
            _context = context;
        }

        // Get all movies
        public async Task<ApiResponse<List<MovieResponseDto>>> GetAllMovies(int currentPage, int pageSize)
        {
            var movies = await _context.Movies
                                       .Include(m => m.MovieGenres)
                                       .ThenInclude(mg => mg.Genre)
                                       .Where(m => m.Status == true)
                                       .Skip((currentPage - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            var totalCount = await _context.Movies
                .Where(m => m.Status == true)
                .CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginationInfo = new PaginationInfo(currentPage, totalPages, pageSize, totalCount);
            var movieResponseDto = new List<MovieResponseDto>();
            foreach (var movie in movies)
            {
                movieResponseDto.Add(MapMovieToMovieResponseDto(movie));
            }
            if (movieResponseDto.Count == 0)
            {
                throw new NotFoundException("Movies not found");
            }
            return new ApiResponse<List<MovieResponseDto>>(movieResponseDto, "Películas obtenidas correctamente", paginationInfo);
        }

        // Get movie by id
        public async Task<ApiResponse<MovieResponseDto>> GetMovieById(int id)
        {
            var movie = await _context.Movies
                                    .Include(m => m.MovieGenres)
                                    .ThenInclude(mg => mg.Genre)
                                    .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            return new ApiResponse<MovieResponseDto>(MapMovieToMovieResponseDto(movie), "Película obtenida correctamente");
        }

        // Create movie
        public async Task<ApiResponse<MovieResponseDto>> CreateMovie(MovieCreateDto movieDto)
        {
            var genres = await _context.Genres
                                .Where(g => movieDto.GenreIds.Contains(g.GenreId))
                                .ToListAsync();
            if (genres.Count == 0)
            {
                throw new NotFoundException("Genres not found");
            }

            var movie = new Movies
            {
                TitleMovie = movieDto.TitleMovie,
                ReleaseDate = movieDto.ReleaseDate,
                MovieGenres = genres.Select(g => new MovieGenre
                {
                    Genre = g
                }).ToList(),
                Status = true
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return new ApiResponse<MovieResponseDto>(MapMovieToMovieResponseDto(movie), "Película insertada correctamente");
        }

        // Update movie
        public async Task<ApiResponse<MovieResponseDto>> UpdateMovie(int id, MovieCreateDto movieDto)
        {
            var movieExist = await _context.Movies
                                                .Include(m => m.MovieGenres)
                                                .ThenInclude(mg => mg.Genre)
                                                .FirstOrDefaultAsync(m => m.MovieId == id);

            var genresExist = await _context.Genres
                                                .Where(g => movieDto.GenreIds.Contains(g.GenreId))
                                                .ToListAsync();

            if (movieExist == null)
            {
                throw new NotFoundException("Movie not found");
            }

            // Actualizar los campos del movie
            movieExist.TitleMovie = movieDto.TitleMovie;
            movieExist.ReleaseDate = movieDto.ReleaseDate;

            // Identificar qué relaciones deben ser eliminadas
            var genresToRemove = movieExist.MovieGenres
                                           .ToList();

            // Identificar qué relaciones deben ser eliminadas
            foreach (var genreToRemove in genresToRemove)
            {
                movieExist.MovieGenres.Remove(genreToRemove);
            }

            // Agregar las nuevas relaciones
            foreach (var genreToAdd in genresExist)
            {
                movieExist.MovieGenres.Add(new MovieGenre { MovieId = id, GenreId = genreToAdd.GenreId, Genre = genreToAdd });
            }

            await _context.SaveChangesAsync();
            return new ApiResponse<MovieResponseDto>(MapMovieToMovieResponseDto(movieExist), "Película actualizada correctamente");
        }

        // Delete movie
        public async Task<ApiResponse<String>> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found");
            }
            movie.Status = false;
            await _context.SaveChangesAsync();
            return new ApiResponse<String>("Película eliminada correctamente");
        }

        // Map Movie to MovieResponseDto
        private MovieResponseDto MapMovieToMovieResponseDto(Movies movie)
        {
            return new MovieResponseDto
            {
                MovieId = movie.MovieId,
                TitleMovie = movie.TitleMovie,
                ReleaseDate = movie.ReleaseDate,
                Genres = movie.MovieGenres.Select(mg => new GenreDto
                {
                    GenreId = mg.Genre.GenreId,
                    NameGenre = mg.Genre.NameGenre
                }).ToList()
            };
        }
    }
}
