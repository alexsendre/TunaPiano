using System.Data.Common;
using TunaPiano.Models;

namespace TunaPiano.Requests
{
    public class Artists
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/api/artists", (TunaPianoDbContext db) =>
            {
                return db.Artists.ToList();
            });

            app.MapGet("/api/artists/{id}", (TunaPianoDbContext db, int id) =>
            {
                return db.Artists.SingleOrDefault(a => a.Id == id);
            });

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
        }
    }
}
