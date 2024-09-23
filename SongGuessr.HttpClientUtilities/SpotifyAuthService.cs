using SongGuessr.Shared;
using SongGuessr.Shared.Authentication;
using System.Text.Json;

namespace SongGuessr.HttpClientUtilities;

public class SpotifyAuthService : ISpotifyAuthService
{
    private readonly SpotifySettings _spotifySettings;

    public SpotifyAuthService(SpotifySettings spotifySettings)
    {
        _spotifySettings = spotifySettings;
    }

    public async Task<Result<GetSpotifyAccessTokenResponse>> GetSpotifyAccessTokenAsync()
    {
        using (var httpClient = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_spotifySettings.ClientId}:{_spotifySettings.ClientSecret}"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            ]);

            request.Content = content;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var stringContentResponse = await response.Content.ReadAsStringAsync();

            return Result<GetSpotifyAccessTokenResponse>.Success(
                JsonSerializer.Deserialize<GetSpotifyAccessTokenResponse>(stringContentResponse)
                ?? throw new ArgumentNullException("HttpResponse", "Unable to deserialize Http Response Body"));
        }
    }
}
