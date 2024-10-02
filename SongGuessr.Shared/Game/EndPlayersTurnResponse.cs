using SongGuessr.Shared.Players;
using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared.Game;

public class EndPlayersTurnResponse
{
    public Player Player { get; set; }
    public Track GuessedTrack { get; set; }
    public bool IsGuessed { get; set; }
}
