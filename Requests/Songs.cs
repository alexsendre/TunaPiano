using System.Data.Common;
using TunaPiano.Models;

namespace TunaPiano.Requests
{
    public class Songs
    {
        public static void Map(WebApplication app)
        {
            // get all songs
            app.MapGet("/api/songs", (TunaPianoDbContext db) =>
            {
                return db.Songs.ToList();
            });

            // get specific song
            app.MapGet("/api/songs/{id}", (TunaPianoDbContext db, int id) =>
            {
                return db.Songs.Where(s => s.Id == id).ToList();
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

            // delete a song
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

            // update song
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
        }
    }
}
