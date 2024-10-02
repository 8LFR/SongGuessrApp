namespace SongGuessr.Shared.Tracks;

public class TracksService : ITracksService
{
    public async Task<Result<GetRandomTrackResponse>> GetRandomTrackAsync(
        GetRandomTrackRequest request, CancellationToken cancellationToken)
    {
        if (request?.PlaylistTracks == null || !request.PlaylistTracks.Any())
        {
            return Result<GetRandomTrackResponse>.Failure("Playlist is empty.");
        }

        var random = new Random();
        var randomIndex = random.Next(request.PlaylistTracks.Count);
        var selectedTrack = request.PlaylistTracks.ElementAt(randomIndex);

        return Result<GetRandomTrackResponse>.Success(new GetRandomTrackResponse { Track = selectedTrack });
    }
}
