namespace SongGuessr.Shared.Tracks;

public interface ITracksService
{
    Task<Result<GetRandomTrackResponse>> GetRandomTrackAsync(
        GetRandomTrackRequest request, CancellationToken cancellationToken);
}
