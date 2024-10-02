using SongGuessr.Shared.Playlists;
using SongGuessr.Shared.Tracks;

namespace SongGuessr.Shared.Game;

public class GameService : IGameService
{ 
    private readonly ISpotifyDataService _spotifyDataService;
    private readonly ITracksService _tracksService;

    public GameService(ISpotifyDataService spotifyDataService, ITracksService tracksService)
    {
        _spotifyDataService = spotifyDataService;
        _tracksService = tracksService;
    }

    public async Task<Result<StartGameResponse>> StartGameAsync(
        StartGameRequest request, CancellationToken cancellationToken)
    {
        var getPlaylistResponse = await _spotifyDataService.GetPlaylistAsync(
            new GetPlaylistRequest(request.PlaylistId, request.AccessToken), 
            cancellationToken);

        var playlist = getPlaylistResponse.Value.Tracks.Items.ToList();
        ShufflePlaylist(playlist);

        var tracks = playlist.Select(playlistTrack => playlistTrack.Track).ToList();
        var playerTracks = new List<PlayerTrack>();

        var tasks = request.Players.Select(async player =>
        {
            var getRandomTrackResponse = await _tracksService.GetRandomTrackAsync(new GetRandomTrackRequest(tracks), cancellationToken);
            var randomTrack = getRandomTrackResponse.Value.Track;

            player.Tracks.Add(randomTrack);

            return new PlayerTrack
            {
                PlayerId = player.Id,
                Track = randomTrack
            };
        });

        playerTracks.AddRange(await Task.WhenAll(tasks));

        var response = new StartGameResponse
        {
            PlayerTracks = playerTracks,
            Tracks = tracks
        };

        return Result<StartGameResponse>.Success(response);
    }

    private void ShufflePlaylist(List<PlaylistTrack> playlist)
    {
        var random = new Random();
        var n = playlist.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (playlist[n], playlist[k]) = (playlist[k], playlist[n]);
        }
    }

    public async Task<Result<StartPlayersTurnResponse>> StartPlayersTurnAsync(
        StartPlayersTurnRequest request, CancellationToken cancellationToken)
    {
        var getRandomTrackResponse = await _tracksService.GetRandomTrackAsync(
            new GetRandomTrackRequest(request.Tracks), cancellationToken);

        var randomTrack = getRandomTrackResponse.Value.Track;
        request.Player.Tracks.Add(randomTrack);

        return Result<StartPlayersTurnResponse>.Success(new StartPlayersTurnResponse
        {
            Track = new PlayerTrack
            {
                PlayerId = request.Player.Id,
                Track = randomTrack
            }
        });
    }

    public async Task<Result<EndPlayersTurnResponse>> EndPlayersTurnAsync(EndPlayersTurnRequest request, CancellationToken cancellationToken)
    {
        //var player = request.Player;
        //var guessedSong = request.GuessedSong;

        //if (player.Tracks.Contains(guessedSong))
        //{
        //    player.Tracks[player.Tracks.IndexOf(guessedSong)].GuessedCorrectly = true;
        //}
        //else
        //{
        //    player.Tracks.Remove(guessedSong);
        //}

        //return Result<EndPlayersTurnResponse>.Success(new EndPlayersTurnResponse
        //{
        //    Player = player
        //});

        throw new NotImplementedException();
    }

    public Task<Result<EndGameResponse>> EndGameAsync(EndGameRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
