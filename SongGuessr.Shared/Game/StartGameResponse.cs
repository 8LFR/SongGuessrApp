using SongGuessr.Shared.Playlists;
using SongGuessr.Shared.Tracks;

namespace SongGuessr.Shared.Game;

public class StartGameResponse
{
    public List<PlayerTrack> PlayerTracks { get; set; }
    public List<Track> Tracks { get; set; }
}
