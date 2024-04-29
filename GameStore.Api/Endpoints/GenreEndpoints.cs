using GameStore.Api.Data;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres")
                    .WithParameterValidation();

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync());
        
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Genre? genre = await dbContext.Genres.FindAsync(id);
            return genre is null ?
                   Results.NotFound() : Results.Ok(genre.ToDto());
        });
        return group;
    }
}
