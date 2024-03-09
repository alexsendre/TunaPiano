using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TunaPiano.Models;

namespace TunaPiano.Requests
{
    public class Artists
    {
        public static void Map(WebApplication app)
        {
            // returns list of artists
            app.MapGet("/api/artists", (TunaPianoDbContext db) =>
            {
                return db.Artists.ToList();
            });

            // returns a specific artist information & their songs
            app.MapGet("/api/artists/{id}", (TunaPianoDbContext db, int id) =>
            {
                var artist = db.Artists.Include(a => a.Songs).SingleOrDefault(a => a.Id == id);

                var body = new
                {
                    id = artist.Id,
                    name = artist.Name,
                    age = artist.Age,
                    bio = artist.Bio,
                    songs = artist.Songs.Select(song => new
                    {
                        title = song.Title,
                        album = song.Album,
                        length = song.Length,
                    })
                };

                return Results.Ok(body);
            });

            // creates an artist
            app.MapPost("/api/artists", (TunaPianoDbContext db, Artist newArtist) =>
            {
                try
                {
                    db.Artists.Add(newArtist);
                    db.SaveChanges();
                    return Results.Created($"/api/artists/{newArtist.Id}", newArtist);
                }
                catch (DbException)
                {
                    return Results.NotFound("Invalid data submitted");
                }
            });

            // deletes a specific artist
            app.MapDelete("/api/artists/{id}", (TunaPianoDbContext db, int id) =>
            {
                var artist = db.Artists.SingleOrDefault(a => a.Id == id);

                if (artist == null)
                {
                    return Results.NotFound("There was an error, the data you requested does not exist");
                }

                db.Artists.Remove(artist);
                db.SaveChanges();
                return Results.NoContent();
            });

            // updates a specific artist
            app.MapPut("/api/artists/{id}", (TunaPianoDbContext db, int id, Artist updateInfo) =>
            {
                var artistToUpdate = db.Artists.SingleOrDefault(a => a.Id == id);

                if (artistToUpdate == null)
                {
                    return Results.NotFound("There was an error, the data you requested was not found");
                }

                artistToUpdate.Name = updateInfo.Name;
                artistToUpdate.Age = updateInfo.Age;
                artistToUpdate.Bio = updateInfo.Bio;

                db.SaveChanges();
                return Results.NoContent();
            });

            // returns a list of a specific artist's songs
            app.MapGet("/api/artists/{id}/songs", (TunaPianoDbContext db, int id) =>
            {
                var artistSongs = db.Songs.Where(a => a.ArtistId == id);
                return artistSongs.ToList();
            });

            // returns artist based on genre 
            app.MapGet("/api/artists/search/genre", (TunaPianoDbContext db, string query) =>
            {
                var genre = db.Genres.FirstOrDefault(g => g.Description.ToLower() == query.ToLower());
                var relatedArtists = db.Artists
                    .Join(db.Songs.Include(s => s.Artist), artist => artist.Id, song => song.ArtistId, (artist, song) => new { artist, song })
                    .Where(result => result.song.Genres.Contains(genre))
                    .Select(result => result.artist);

                return relatedArtists;
            });

            // returns a specific artist's related artists based on genre
            app.MapGet("/api/artists/{id}/related", (TunaPianoDbContext db, int id) =>
            {
                var artist = db.Artists.Include(a => a.Songs).ThenInclude(s => s.Genres).FirstOrDefault(a => a.Id == id);
                if (artist == null)
                {
                    return Results.NotFound();
                }

                var artistGenres = artist.Songs.SelectMany(s => s.Genres).Select(g => g.Description).ToList();
                var relatedArtists = db.Artists
                                .Where(a => a.Id != id && a.Songs.Any(s => s.Genres.Any(g => artistGenres.Contains(g.Description))))
                                .Select(a => new
                                {
                                    id = a.Id,
                                    name = a.Name
                                })
                                .ToList();

                var response = new
                {
                    artists = relatedArtists
                };

                return Results.Ok(response);
            });
        }
    }
}
