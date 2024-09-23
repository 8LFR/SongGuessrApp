using StackExchange.Redis;

namespace SongGuessr.Shared.Lobbys;

public class LobbysService : ILobbysService
{
    private readonly IConnectionMultiplexer _redis;

    public LobbysService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<Result<CreateLobbyResponse>> CreateLobbyAsync(
        CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var lobbyId = Guid.NewGuid().ToString();
        var lobbyKey = $"lobby:{lobbyId}";

        if (await db.KeyExistsAsync(lobbyKey))
        {
            return Result<CreateLobbyResponse>.Failure("Lobby with the same ID already exists.");
        }

        var lobbyData = new HashEntry[]
        {
            new("LobbyId", lobbyId),
            new("LobbyName", request.LobbyName),
            new("IsPublic", request.IsPublic),
            new("Host", request.PlayerName),
            new("CreatedAt", DateTime.UtcNow.ToString())
        };
        await db.HashSetAsync(lobbyKey, lobbyData);

        await db.SetAddAsync("lobbies", lobbyId);

        var playerKey = $"lobby:{lobbyId}:players";
        await db.SetAddAsync(playerKey, request.PlayerName);

        return Result<CreateLobbyResponse>.Success(new CreateLobbyResponse { LobbyId = lobbyId });
    }

    public async Task<Result<GetLobbyResponse>> GetLobbyAsync(
        GetLobbyRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var lobbyKey = $"lobby:{request.LobbyId}";

        if (!await db.KeyExistsAsync(lobbyKey))
        {
            return Result<GetLobbyResponse>.Failure("Lobby does not exist.");
        }

        var lobbyName = await db.HashGetAsync(lobbyKey, "LobbyName");
        var hostName = await db.HashGetAsync(lobbyKey, "Host");

        var playerKey = $"lobby:{request.LobbyId}:players";
        var players = (await db.SetMembersAsync(playerKey)).Select(p => (string)p).ToList();

        var lobby = new LobbyInfo
        {
            LobbyId = request.LobbyId,
            LobbyName = lobbyName,
            Players = players,
            HostName = hostName
        };

        return Result<GetLobbyResponse>.Success(new GetLobbyResponse { Lobby = lobby });
    }

    public async Task<Result<GetAllLobbysResponse>> GetAllLobbysAsync(
        GetAllLobbysRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var lobbyIds = await db.SetMembersAsync("lobbies");

        var lobbies = new List<LobbyInfo>();

        foreach (var lobbyId in lobbyIds)
        {
            var lobbyKey = $"lobby:{lobbyId}";
            var lobbyData = await db.HashGetAllAsync(lobbyKey);

            if (lobbyData.Length == 0) continue;

            var lobbyInfo = new LobbyInfo
            {
                LobbyId = lobbyId,
                LobbyName = lobbyData.FirstOrDefault(x => x.Name == "LobbyName").Value,
                IsPublic = (bool)lobbyData.FirstOrDefault(x => x.Name == "IsPublic").Value,
                CreatedAt = DateTime.Parse(lobbyData.FirstOrDefault(x => x.Name == "CreatedAt").Value)
            };

            lobbies.Add(lobbyInfo);
        }

        return Result<GetAllLobbysResponse>.Success(new GetAllLobbysResponse { Lobbys = lobbies });
    }

    public async Task<Result<JoinLobbyResponse>> JoinLobbyAsync(
        JoinLobbyRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var lobbyKey = $"lobby:{request.LobbyId}";

        if (!await db.KeyExistsAsync(lobbyKey))
        {
            return Result<JoinLobbyResponse>.Failure("Lobby does not exist.");
        }

        var playerKey = $"lobby:{request.LobbyId}:players";
        await db.SetAddAsync(playerKey, request.PlayerName);

        var response = new JoinLobbyResponse
        {
            LobbyId = request.LobbyId,
            PlayerName = request.PlayerName,
        };

        return Result<JoinLobbyResponse>.Success(response);
    }
}
