using ApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Context
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure the properties of the table Movies
            modelBuilder.Entity<Movies>(entity =>
            {
                entity.HasKey(e => e.MovieId);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            //Configure the properties of the table Genres
            modelBuilder.Entity<Genres>(entity =>
            {
                entity.HasKey(e => e.GenreId);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            //Configure the properties of the table MovieGenre
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);
        }

    }
}
