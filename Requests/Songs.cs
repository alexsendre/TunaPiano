using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TunaPiano.Models;

namespace TunaPiano.Requests
{
    public class Songs
    {
        public static void Map(WebApplication app)
        {
            // returns a list of songs
            app.MapGet("/api/songs", (TunaPianoDbContext db) =>
            {
                return db.Songs.ToList();
            });

            // returns a specific song with associated data
            app.MapGet("/api/songs/{id}", (TunaPianoDbContext db, int id) =>
            {
                var song = db.Songs.Include(s => s.Genres).Include(s => s.Artist).SingleOrDefault(s => s.Id == id);

                var body = new
                {
                    id = song.Id,
                    title = song.Title,
                    artist = new
                    {
                        id = song.Artist.Id,
                        name = song.Artist.Name,
                        age = song.Artist.Age,
                        bio = song.Artist.Bio,
                    },
                    album = song.Album,
                    length = song.Length,
                    genre = song.Genres.Select(genre => new
                    {
                        description = genre.Description,
                    }),
                };

                return Results.Ok(body);
            });

            // create a song
            app.MapPost("/api/songs", (TunaPianoDbContext db, Song newSong) =>
            {
                try
                {
                    db.Songs.Add(newSong);
                    db.SaveChanges();
                    return Results.Created($"/api/songs/{newSong.Id}", newSong);
                }
                catch (DbException)
                {
                    return Results.BadRequest("Something went wrong, invalid data submitted.");
                }
            });

            // delete a specific song
            app.MapDelete("/api/songs/{id}", (TunaPianoDbContext db, int id) =>
            {
                var song = db.Songs.FirstOrDefault(s => s.Id == id);

                if (song == null)
                {
                    return Results.NotFound("Invalid data requested, song was not found.");
                }

                db.Songs.Remove(song);
                db.SaveChanges();
                return Results.NoContent();
            });

            // update a specific song
            app.MapPut("/api/songs/{id}/edit", (TunaPianoDbContext db, int id, Song updateInfo) =>
            {
                Song songToUpdate = db.Songs.SingleOrDefault(s => s.Id == id);

                if (songToUpdate == null)
                {
                    return Results.NotFound("There was an error");
                }

                songToUpdate.Title = updateInfo.Title;
                songToUpdate.Album = updateInfo.Album;
                songToUpdate.Length = updateInfo.Length;

                db.SaveChanges();
                return Results.NoContent();
            });

            // update song with genres
            app.MapPut("/api/songs/{songId}/genres/{genreId}", (TunaPianoDbContext db, int songId, int genreId) =>
            {
                var song = db.Songs.FirstOrDefault(s => s.Id == songId);
                var genre = db.Genres.Find(genreId);

                if (song == null || genre == null)
                {
                    return Results.NotFound("Could not find requested data");
                }

                if (song.Genres == null)
                {
                    song.Genres = new List<Genre>();
                }

                song.Genres.Add(genre);
                db.SaveChanges();
                return Results.Ok();
            });

            // returns a songs that is associated by search query 
            app.MapGet("/api/songs/search/genre", (TunaPianoDbContext db, string query) =>
            {
                var genre = db.Genres.FirstOrDefault(g => g.Description.ToLower() == query.ToLower());
                var relatedSongs = db.Songs.Where(result => result.Genres.Contains(genre));

                return relatedSongs;
            });
        }
    }
}
