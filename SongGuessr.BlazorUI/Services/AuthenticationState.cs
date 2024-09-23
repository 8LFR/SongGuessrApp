using SongGuessr.Shared.Playlists;

namespace SongGuessr.BlazorUI.Services;

public class AuthenticationState
{
    public string AccessToken { get; set; }
    public GetPlaylistResponse GetPlaylistResponse { get; set; }
    public string PlayerName { get; set; }
    public bool IsHost { get; set; } = false;
}
