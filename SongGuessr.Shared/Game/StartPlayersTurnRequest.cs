using SongGuessr.Shared.Players;
using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared.Game;

public record StartPlayersTurnRequest(Player Player, List<Track> Tracks);
