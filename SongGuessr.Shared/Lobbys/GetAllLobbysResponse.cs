namespace SongGuessr.Shared.Lobbys;

public class GetAllLobbysResponse
{
    public IReadOnlyCollection<LobbyInfo?> Lobbys { get; set; }
}
