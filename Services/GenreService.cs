using ApiTest.Classes;
using ApiTest.Context;
using ApiTest.DTOs.Genres;
using ApiTest.DTOs.Movies;
using ApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Services
{
    public class GenreService
    {
        private readonly AppDBContext _context;

        public GenreService(AppDBContext context)
        {
            _context = context;
        }

        // Get all genres
        public async Task<ApiResponse<List<GenreDto>>> GetAllGenres(int currentPage, int pageSize)
        {
            var genres = await _context.Genres
                .OrderBy(g => g.NameGenre)
                .Where(g => g.Status == true)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalCount = await _context.Genres
                .Where(g => g.Status == true)
                .CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginationInfo = new PaginationInfo(currentPage, totalPages, pageSize, totalCount);
            var genreDtos = genres.Select(genre => new GenreDto
            {
                GenreId = genre.GenreId,
                NameGenre = genre.NameGenre
            }).ToList();

            if (genreDtos.Count==0)
            {
                throw new NotFoundException("Genres not found");
            }

            return new ApiResponse<List<GenreDto>>(genreDtos, "Géneros obtenidos con éxito.", paginationInfo);
        }

        // Get genre by id
        public async Task<ApiResponse<GenreDto>> GetGenreById(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId==id);
            if(genre == null)
            {
                throw new NotFoundException("Genre not found");
            }
            var genreDto = MapGenreToGenreDto(genre);
            return new ApiResponse<GenreDto>(genreDto, "Género obtenido con éxito.");
        }

        // Create genre
        public async Task<ApiResponse<GenreDto>> CreateGenre(GenreCreateDto genreDto)
        {
            var genre = new Genres
            {
                NameGenre = genreDto.NameGenre,
                Status = true
            };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            var genreResponseDto = new GenreDto
            {
                GenreId = genre.GenreId,
                NameGenre = genre.NameGenre
            };
            return new ApiResponse<GenreDto>(genreResponseDto, "Género insertado correctamente");
        }

        // Update genre
        public async Task<ApiResponse<GenreDto>> UpdateGenre(int id, GenreCreateDto genre)
        {
            var genreExist = await _context.Genres.FindAsync(id);
            if (genreExist == null)
            {
                throw new NotFoundException("Genre not found");
            }
            genreExist.NameGenre = genre.NameGenre;
            await _context.SaveChangesAsync();
            return new ApiResponse<GenreDto>(MapGenreToGenreDto(genreExist), "Género actualizado correctamente");
        }

        // Delete genre
        public async Task<ApiResponse<String>> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                throw new NotFoundException("Genre not found");
            }
            genre.Status = false;
            await _context.SaveChangesAsync();
            return new ApiResponse<String>("Género eliminado correctamente");
        }

        // Map genre to genre dto
        private GenreDto MapGenreToGenreDto(Genres genre)
        {
            return new GenreDto
            {
                GenreId = genre.GenreId,
                NameGenre = genre.NameGenre
            };
        }
    }

}
