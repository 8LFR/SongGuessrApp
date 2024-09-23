using SQLite;

namespace SongGuessr.Shared;

public class Song
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string SpotifyLink { get; set; }
}
