namespace SongGuessr.Shared.Lobbys;

public interface ILobbysService
{
    Task<Result<CreateLobbyResponse>> CreateLobbyAsync(
        CreateLobbyRequest request, CancellationToken cancellationToken);

    Task<Result<GetLobbyResponse>> GetLobbyAsync(
        GetLobbyRequest request, CancellationToken cancellationToken);

    Task<Result<GetAllLobbysResponse>> GetAllLobbysAsync(
        GetAllLobbysRequest request, CancellationToken cancellationToken);

    Task<Result<JoinLobbyResponse>> JoinLobbyAsync(
        JoinLobbyRequest request, CancellationToken cancellationToken);
}
