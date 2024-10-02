namespace SongGuessr.Shared.Players;

public class GetPlayersResponse
{
    public IReadOnlyCollection<Player> Players { get; set;} = [];
}
