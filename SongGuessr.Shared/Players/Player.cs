using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared.Players;

public class Player
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Track> Tracks { get; set; } = [];
}
