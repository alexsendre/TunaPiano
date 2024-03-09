using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TunaPiano.Models;

namespace TunaPiano.Requests
{
    public class Genres
    {
        public static void Map(WebApplication app)
        {
            // returns a list of genres
            app.MapGet("/api/genres", (TunaPianoDbContext db) =>
            {
                return db.Genres.ToList();
            });

            // returns a specific genre and songs associated
            app.MapGet("/api/genres/{id}", (TunaPianoDbContext db, int id) =>
            {
                var genre = db.Genres.Include(g => g.Songs).SingleOrDefault(g => g.Id == id);

                if (genre == null)
                {
                    return Results.NotFound("This genre does not exist, try another");
                }

                var body = new
                {
                    id = genre.Id,
                    description = genre.Description,
                    songs = genre.Songs.Select(song => new
                    {
                        title = song.Title,
                        album = song.Album,
                        length = song.Length,
                    })
                };

                return Results.Ok(body);
            });

            // create a genre
            app.MapPost("/api/genres", (TunaPianoDbContext db, Genre newGenre) =>
            {
                try
                {
                    db.Genres.Add(newGenre);
                    db.SaveChanges();
                    return Results.Created($"/api/genres/{newGenre.Id}", newGenre);
                }
                catch (DbException)
                {
                    return Results.BadRequest("Something went wrong, invalid data submitted.");
                }
            });

            // delete specific genre
            app.MapDelete("/api/genres/{id}", (TunaPianoDbContext db, int id) =>
            {
                var genre = db.Genres.FirstOrDefault(g => g.Id == id);

                if (genre == null)
                {
                    return Results.NotFound("Invalid data requested");
                }

                db.Genres.Remove(genre);
                db.SaveChanges();
                return Results.NoContent();
            });

            // update specific genre
            app.MapPut("/api/genres/{id}", (TunaPianoDbContext db, int id, Genre updateInfo) =>
            {
                var genreToUpdate = db.Genres.FirstOrDefault(g => g.Id == id);

                if (genreToUpdate == null)
                {
                    return Results.NotFound("Invalid data requested- could not find.");
                }

                genreToUpdate.Description = updateInfo.Description;

                db.SaveChanges();
                return Results.NoContent();
            });

            // returns an ordered list based on songs within a genre (most popular at top)
            app.MapGet("/api/genres/popular", (TunaPianoDbContext db) =>
            {
                var popular = db.Genres
                    .Select(genre => new
                    {
                        id = genre.Id,
                        description = genre.Description,
                        song_count = genre.Songs.Count()
                    }).OrderByDescending(genre => genre.song_count).ToList();
                
                return Results.Ok(new { genres = popular });
            });
        }
    }
}
