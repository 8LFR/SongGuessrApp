using SongGuessr.HttpClientUtilities;
using SongGuessr.Shared;
using SongGuessr.Shared.Authentication;
using SongGuessr.Shared.Game;
using SongGuessr.Shared.Lobbys;
using SongGuessr.Shared.Players;
using SongGuessr.Shared.Playlists;
using SongGuessr.Shared.Tracks;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SpotifySettings>(builder.Configuration.GetSection("SpotifySettings"));
var spotifySettings = builder.Configuration.GetSection("SpotifySettings").Get<SpotifySettings>();
builder.Services.AddSpotifyHttpClientServices(spotifySettings);

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["RedisSettings:ConnectionString"]));
builder.Services.AddSingleton<ISpotifyAuthService, SpotifyAuthService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IPlaylistsService, PlaylistsService>();
builder.Services.AddTransient<ILobbysService, LobbysService>();
builder.Services.AddTransient<ITracksService, TracksService>();
builder.Services.AddTransient<IPlayersService, PlayersService>();
builder.Services.AddTransient<IGameService, GameService>();

var configuration = builder.Configuration;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
