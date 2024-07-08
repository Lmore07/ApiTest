using System.ComponentModel.DataAnnotations;

namespace ApiTest.Models
{
    public class Movies
    {
        public int MovieId { get; set; }
        public string TitleMovie { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Boolean Status { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }

    }
}
