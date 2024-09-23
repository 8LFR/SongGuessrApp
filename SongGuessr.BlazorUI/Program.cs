using SongGuessr.BlazorUI.Components;
using SongGuessr.BlazorUI.Services;
using SongGuessr.HttpClientUtilities;
using SongGuessr.Shared;
using SongGuessr.Shared.Authentication;
using SongGuessr.Shared.Lobbys;
using SongGuessr.Shared.Playlists;
using SongGuessr.Shared.Tracks;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<SpotifySettings>(builder.Configuration.GetSection("SpotifySettings"));
var spotifySettings = builder.Configuration.GetSection("SpotifySettings").Get<SpotifySettings>();
builder.Services.AddSpotifyHttpClientServices(spotifySettings);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["RedisSettings:ConnectionString"]));
builder.Services.AddSingleton<AuthenticationState>();
builder.Services.AddSingleton<ISpotifyAuthService, SpotifyAuthService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ILobbysService, LobbysService>();
builder.Services.AddTransient<IPlaylistsService, PlaylistsService>();
builder.Services.AddTransient<ITracksService, TracksService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
