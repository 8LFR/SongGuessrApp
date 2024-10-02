namespace SongGuessr.Shared.Lobbys;

public record CreateLobbyRequest(string LobbyName, bool IsPublic, string PlayerId);
