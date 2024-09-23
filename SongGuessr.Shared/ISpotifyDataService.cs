using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared;

public interface ISpotifyDataService
{
    Task<Result<GetPlaylistResponse>> GetPlaylistAsync(
        GetPlaylistRequest request, 
        CancellationToken cancellationToken);
}
