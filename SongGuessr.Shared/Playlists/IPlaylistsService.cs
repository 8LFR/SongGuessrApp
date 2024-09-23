namespace SongGuessr.Shared.Playlists;

public interface IPlaylistsService
{
    Task<Result<GetPlaylistResponse>> GetPlaylistAsync(
        GetPlaylistRequest request, CancellationToken cancellationToken);
}
