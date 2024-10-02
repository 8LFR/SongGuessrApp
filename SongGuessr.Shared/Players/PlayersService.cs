using StackExchange.Redis;

namespace SongGuessr.Shared.Players;

public class PlayersService(IConnectionMultiplexer redis) : IPlayersService
{
    private readonly IConnectionMultiplexer _redis = redis;

    public async Task<Result<CreatePlayerResponse>> CreatePlayerAsync(
        CreatePlayerRequest request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var playerKey = "players";

        var player = new Player
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name
        };

        await db.SetAddAsync(playerKey, player.Id);

        var playerDetailsKey = $"player:{player.Id}";
        await db.HashSetAsync(playerDetailsKey,
        [
            new("Id", player.Id),
            new("Name", player.Name)
        ]);

        return Result<CreatePlayerResponse>.Success(new CreatePlayerResponse { Player = player });
    }

    public async Task<Result<GetPlayersResponse>> GetAllPlayersAsync()
    {
        var db = _redis.GetDatabase();
        var playerKey = "players";

        var playerIds = await db.SetMembersAsync(playerKey);
        var players = new List<Player>();

        foreach (var playerId in playerIds)
        {
            var playerDataKey = $"player:{playerId}";

            var playerFields = await db.HashGetAllAsync(playerDataKey);

            if (playerFields.Length > 0)
            {
                var player = new Player
                {
                    Id = playerFields.First(x => x.Name == "Id").Value,
                    Name = playerFields.First(x => x.Name == "Name").Value
                };

                players.Add(player);
            }
        }

        return Result<GetPlayersResponse>.Success(new GetPlayersResponse { Players = players });
    }
}
