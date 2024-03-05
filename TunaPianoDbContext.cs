using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;

namespace TunaPiano
{
    public class TunaPianoDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasData(new Artist[]
            {
                new Artist { Id = 1, Name = "Travis Scott", Age = 33, Bio = "belt" },
                new Artist { Id = 2, Name = "Jean Dawson", Age = 27, Bio = "genre defying artist wow so good" },
                new Artist { Id = 3, Name = "No Rome", Age = 31, Bio = "smooth chill sometimes hype sometimes cry" },
                new Artist { Id = 4, Name = "Dominic Fike", Age = 28, Bio = "dominic fike brooo" },
            });

            modelBuilder.Entity<Song>().HasData(new Song[]
            {
                new Song { Id = 1, Title = "Devilish", ArtistId = 2, Album = "Pixel Bath", Length = 3 },
                new Song { Id = 2, Title = "Sirens", ArtistId = 1, Album = "Utopia", Length = 3 },
                new Song { Id = 3, Title = "ITS *NOT* L0V33 (Winter in London)", ArtistId = 3, Album = "It's All Smiles", Length = 2 },
                new Song { Id = 4, Title = "the ends", ArtistId = 1, Album = "Birds in the Trap Sing McKnight", Length = 4 },
                new Song { Id = 5, Title = "When She Comes Around", ArtistId = 3, Album = "It's All Smiles", Length = 3 },
                new Song { Id = 6, Title = "Dancing In The Courthouse", ArtistId = 4, Album = "Sunburn", Length = 2 },
            });

            modelBuilder.Entity<Genre>().HasData(new Genre[]
            {
                new Genre { Id = 1, Description = "Hip-Hop/Rap" },
                new Genre { Id = 2, Description = "Alternative" },
                new Genre { Id = 3, Description = "Indie Pop" },
            });
        }
    }
}
