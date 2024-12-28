using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEnpointName = "GetGame";

List<GameDto> games=[
    new(1,"God of war","Fighting",20.00M,new DateOnly(1992,7,15)),
    new(2,"Fifa 24","Sports",26.00M,new DateOnly(1992,7,15)),
    new(3,"God of war","Fighting",20.00M,new DateOnly(1992,7,15)),
    new(4,"God of war","Fighting",20.00M,new DateOnly(1992,7,15))
];
app.MapGet("/games",() => games);
app.MapGet("/games/{id}",(int id)=>{

    GameDto? game = games.Find(game=>game.Id==id);
    return game is null? Results.NotFound() : Results.Ok(game);
    })
    .WithName(GetGameEnpointName);


app.MapPost("/games",(CreateGameDto newGame)=>{
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
app.MapPut("/games/{id}",(int id,UpdateGameDto updatedGame)=>{
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
app.MapDelete("/games/{id}",(int id)=>{
    games.RemoveAll(game=>game.Id == id);
    return Results.Ok();
});

app.Run();
