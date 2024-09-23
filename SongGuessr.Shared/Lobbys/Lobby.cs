namespace SongGuessr.Shared.Lobbys;

public class Lobby
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public string HostPlayer { get; set; }
    public DateTime CreatedAt { get; set; }
}
