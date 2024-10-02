namespace SongGuessr.Shared.Game;

public interface IGameService
{
    Task<Result<StartGameResponse>> StartGameAsync(
        StartGameRequest request, CancellationToken cancellationToken);

    Task<Result<StartPlayersTurnResponse>> StartPlayersTurnAsync(
        StartPlayersTurnRequest request, CancellationToken cancellationToken);

    // In end players turn
    // check if player guessed the song
    // if player guessed the song, add points to player and keep track in his tracks
    // if player didn't guess the song, remove track from his tracks
    Task<Result<EndPlayersTurnResponse>> EndPlayersTurnAsync(
               EndPlayersTurnRequest request, CancellationToken cancellationToken);

    Task<Result<EndGameResponse>> EndGameAsync(
        EndGameRequest request, CancellationToken cancellationToken);
}
