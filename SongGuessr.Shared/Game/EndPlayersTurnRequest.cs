using SongGuessr.Shared.Players;
using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared.Game;

public record EndPlayersTurnRequest(Player Player, Track GuessedSong, string? ArtistName, string? TrackName);
