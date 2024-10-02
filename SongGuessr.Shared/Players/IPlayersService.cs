namespace SongGuessr.Shared.Players;

public interface IPlayersService
{
    Task<Result<GetPlayersResponse>> GetAllPlayersAsync();

    Task<Result<CreatePlayerResponse>> CreatePlayerAsync(
        CreatePlayerRequest request, CancellationToken cancellationToken);
}
