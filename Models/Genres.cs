namespace ApiTest.Models
{
    public class Genres
    {
        public int GenreId { get; set; }
        public string NameGenre { get; set; }
        public Boolean Status { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }
    }
}
