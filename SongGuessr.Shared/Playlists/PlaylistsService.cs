namespace SongGuessr.Shared.Playlists;

public class PlaylistsService : IPlaylistsService
{
    private readonly ISpotifyDataService _spotifyDataService;

    public PlaylistsService(ISpotifyDataService spotifyDataService)
    {
        _spotifyDataService = spotifyDataService;
    }

    public async Task<Result<GetPlaylistResponse>> GetPlaylistAsync(
        GetPlaylistRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _spotifyDataService.GetPlaylistAsync(request, cancellationToken);
    }
}
