using SongGuessr.Shared.Players;

namespace SongGuessr.Shared.Game;

public record StartGameRequest(IReadOnlyCollection<Player> Players, string PlaylistId, string AccessToken);