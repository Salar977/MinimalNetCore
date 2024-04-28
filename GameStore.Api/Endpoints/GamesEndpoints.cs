using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

// extention class
public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    
    private static readonly List<GameDto> games = [
        new (
            Id: 1,
            Name: "Street Fighter II",
            Genre: "Fighting",
            Price: 19.99m,
            ReleaseYear: new DateOnly(1992, 7, 15)
        ),
        new (
            Id: 2,
            Name: "Final Fantasy XIV",
            Genre: "Roleplaying",
            Price: 59.99m,
            ReleaseYear: new DateOnly(2010, 9, 30)
        ),
        new (
            Id: 3,
            Name: "FIFA 23",
            Genre: "Sports",
            Price: 69.99m,
            ReleaseYear: new DateOnly(2022, 9, 27)
        )
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                       .WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(x => x.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        // Post /games
        group.MapPost("/", (CreateGameDto newGame) => {
            GameDto game = new (
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseYear
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1) return Results.NotFound();

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseYear
            );

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            
            return Results.NoContent();
        });
        return group;
    }
}
