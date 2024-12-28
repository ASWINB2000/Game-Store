using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
const string GetGameEnpointName = "GetGame";

private static readonly List<GameDto> games=[
    new(1,"God of war","Fighting",20.00M,new DateOnly(1992,7,15)),
    new(2,"Fifa 24","Sports",26.00M,new DateOnly(1992,7,15)),
    new(3,"God of war","Fighting",20.00M,new DateOnly(1992,7,15)),
    new(4,"God of war","Fighting",20.00M,new DateOnly(1992,7,15))
    ];
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        group.MapGet("/",() => games);
    group.MapGet("/{id}",(int id)=>{

        GameDto? game = games.Find(game=>game.Id==id);
        return game is null? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEnpointName);


    group.MapPost("/",(CreateGameDto newGame)=>{
         
        GameDto game = new(
            games.Count + 1,
            newGame.Name, 
            newGame.Genre,
            newGame.Price,
            newGame.ReleaseDate
        );
        games.Add(game);
        return Results.CreatedAtRoute(GetGameEnpointName,new {id = game.Id},game);
    });
    group.MapPut("/{id}",(int id,UpdateGameDto updatedGame)=>{
        var index = games.FindIndex(game=>game.Id == id);
        if(index == -1)
        {
            return Results.NotFound(); // you could also create a new resource instead of just returning not found.
        }
        games[index] = new GameDto(
            id,
            updatedGame.Name,
            updatedGame.Genre,
            updatedGame.Price,
            updatedGame.ReleaseDate
        );
        return Results.Json(games[index]);
    });
    group.MapDelete("/{id}",(int id)=>{
        games.RemoveAll(game=>game.Id == id);
        return Results.Ok();
    });
    return group;
    }
}
