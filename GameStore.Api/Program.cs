using System;
using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
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
    ),
];

// GET /games
app.MapGet("games", () => games);

// GET /games/{id}
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName("GetGameById");

// Post /games
app.MapPost("games", (CreateGameDto newGame) => {
    GameDto game = new (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseYear
    );
    games.Add(game);
    return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, game);
});

// PUT /games/{id}
app.MapPut("games/{id}", (int id, GameDto game) => {
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1) {
        return Results.NotFound();
    }
    games[index] = game;
    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("games/{id}", (int id) => {
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1) {
        return Results.NotFound();
    }
    games.RemoveAt(index);
    return Results.NoContent();
});

app.Run();