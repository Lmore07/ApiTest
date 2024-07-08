using ApiTest.DTOs.Genres;

namespace ApiTest.DTOs.Movies
{
    public class MovieCreateDto
    {
        public string TitleMovie { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class MovieResponseDto
    {
        public int MovieId { get; set; }
        public string TitleMovie { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<GenreDto> Genres { get; set; }
    }

    public class MovieByGenreResponseDto
    {
        public int MovieId { get; set; }
        public string TitleMovie { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}
