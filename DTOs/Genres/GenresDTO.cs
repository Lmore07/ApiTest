namespace ApiTest.DTOs.Genres
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public string NameGenre { get; set; }
    }

    public class GenreCreateDto
    {
        public string NameGenre { get; set; }
    }
}
