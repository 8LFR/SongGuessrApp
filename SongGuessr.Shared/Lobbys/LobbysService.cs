using SongGuessr.Shared.Players;
using StackExchange.Redis;

namespace SongGuessr.Shared.Lobbys;

public class LobbysService(IConnectionMultiplexer redis) : ILobbysService
{
    private readonly IConnectionMultiplexer _redis = redis;

    public async Task<Result<CreateLobbyResponse>> CreateLobbyAsync(
        CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var playerKey = $"players";
        var playerId = request.PlayerId;

        if (!await db.SetContainsAsync(playerKey, playerId))
        {
            return Result<CreateLobbyResponse>.Failure("Player does not exist in the database.");
        }

        var lobbyId = Guid.NewGuid().ToString();
        var lobbyKey = $"lobby:{lobbyId}";

        var transaction = db.CreateTransaction();

        var lobbyData = new HashEntry[]
        {
            new("LobbyId", lobbyId),
            new("LobbyName", request.LobbyName),
            new("IsPublic", request.IsPublic.ToString()),
            new("Host", playerId),
            new("CreatedAt", DateTime.UtcNow.ToString("O"))
        };

        transaction.AddCondition(Condition.KeyNotExists(lobbyKey));

        var hashSetTask = transaction.HashSetAsync(lobbyKey, lobbyData);
        var addToLobbiesTask = transaction.SetAddAsync("lobbies", lobbyId);
        var addPlayerToLobbyTask = transaction.SetAddAsync($"lobby:{lobbyId}:players", playerId);

        if (await transaction.ExecuteAsync())
        {
            await Task.WhenAll(hashSetTask, addToLobbiesTask, addPlayerToLobbyTask);
            return Result<CreateLobbyResponse>.Success(new CreateLobbyResponse { LobbyId = lobbyId });
        }
        else
        {
            return Result<CreateLobbyResponse>.Failure("Failed to create lobby. Please try again.");
        }
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

        var lobbyData = await db.HashGetAllAsync(lobbyKey);

        var lobby = new LobbyInfo
        {
            LobbyId = request.LobbyId,
            LobbyName = lobbyData.FirstOrDefault(x => x.Name == "LobbyName").Value,
            HostName = lobbyData.FirstOrDefault(x => x.Name == "Host").Value,
            IsPublic = bool.Parse(lobbyData.FirstOrDefault(x => x.Name == "IsPublic").Value.ToString() ?? "false"),
            CreatedAt = DateTime.TryParse(
                lobbyData.FirstOrDefault(x => x.Name == "CreatedAt").Value,
                out var createdAt) ? createdAt : DateTime.MinValue
        };

        var playerKey = $"lobby:{request.LobbyId}:players";
        var players = await db.SetMembersAsync(playerKey);
        lobby.Players = players.Select(p => new Player { Name = (string)p }).ToList();

        return Result<GetLobbyResponse>.Success(new GetLobbyResponse { Lobby = lobby });
    }

    public async Task<Result<GetAllLobbysResponse>> GetAllLobbysAsync(
        GetAllLobbysRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var lobbyIds = await db.SetMembersAsync("lobbies");
        var lobbies = new List<LobbyInfo>();
        var tasks = new List<Task>();

        foreach (var lobbyId in lobbyIds)
        {
            tasks.Add(Task.Run(async () =>
            {
                var lobbyKey = $"lobby:{lobbyId}";
                var lobbyData = await db.HashGetAllAsync(lobbyKey);
                if (lobbyData.Length == 0) return;

                var lobbyInfo = new LobbyInfo
                {
                    LobbyId = lobbyId,
                    LobbyName = lobbyData.FirstOrDefault(x => x.Name == "LobbyName").Value,
                    HostName = lobbyData.FirstOrDefault(x => x.Name == "Host").Value,
                    IsPublic = bool.Parse(lobbyData.FirstOrDefault(x => x.Name == "IsPublic").Value.ToString() ?? "false"),
                    CreatedAt = DateTime.TryParse(
                        lobbyData.FirstOrDefault(x => x.Name == "CreatedAt").Value,
                        out var createdAt) ? createdAt : DateTime.MinValue
                };

                var playerKey = $"lobby:{lobbyId}:players";
                var players = await db.SetMembersAsync(playerKey);

                var playerDetailsTasks = players.Select(async p =>
                {
                    var playerId = (string)p;
                    var playerDataKey = $"player:{playerId}";

                    var playerData = await db.HashGetAllAsync(playerDataKey);
                    if (playerData.Length == 0) return null;

                    return new Player
                    {
                        Id = playerData.FirstOrDefault(x => x.Name == "Id").Value,
                        Name = playerData.FirstOrDefault(x => x.Name == "Name").Value
                    };
                });

                var playerList = await Task.WhenAll(playerDetailsTasks);
                lobbyInfo.Players = playerList.Where(p => p != null).ToList();

                lock (lobbies)
                {
                    lobbies.Add(lobbyInfo);
                }
            }, cancellationToken));
        }

        await Task.WhenAll(tasks);

        return Result<GetAllLobbysResponse>.Success(new GetAllLobbysResponse { Lobbys = lobbies });
    }

    public async Task<Result<JoinLobbyResponse>> JoinLobbyAsync(
        JoinLobbyRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var playerKey = $"players";
        var playerName = request.PlayerId;

        if (!await db.SetContainsAsync(playerKey, playerName))
        {
            return Result<JoinLobbyResponse>.Failure("Player does not exist in the database.");
        }

        var lobbyKey = $"lobby:{request.LobbyId}";

        if (!await db.KeyExistsAsync(lobbyKey))
        {
            return Result<JoinLobbyResponse>.Failure("Lobby does not exist.");
        }

        var playerLobbyKey = $"lobby:{request.LobbyId}:players";
        await db.SetAddAsync(playerLobbyKey, request.PlayerId);

        var response = new JoinLobbyResponse
        {
            LobbyId = request.LobbyId,
            PlayerId = request.PlayerId,
        };

        return Result<JoinLobbyResponse>.Success(response);
    }
}
