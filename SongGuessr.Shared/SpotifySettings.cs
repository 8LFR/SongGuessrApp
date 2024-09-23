namespace SongGuessr.Shared;

public class SpotifySettings
{
    public Uri BaseUri { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUrl { get; set; }
    public int? TimeoutInSeconds { get; set; }
}
