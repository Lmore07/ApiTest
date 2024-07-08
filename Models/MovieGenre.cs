namespace ApiTest.Models
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public Movies Movie { get; set; }

        public int GenreId { get; set; }
        public Genres Genre
        {
            get; set;
        }
    }
}
