using SongGuessr.Shared.Players;

namespace SongGuessr.Shared.Lobbys;

public class LobbyInfo
{
    public string LobbyId { get; set; }
    public string LobbyName { get; set; }
    public IReadOnlyCollection<Player> Players { get; set; }
    public string HostName { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
}
