using Microsoft.Extensions.Logging;
using SongGuessr.Shared;
using SongGuessr.Shared.Playlists;
using System.Net;
using System.Text.Json;

namespace SongGuessr.HttpClientUtilities;

public class SpotifyDataService : ISpotifyDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SpotifyDataService> _logger;

    public SpotifyDataService(
        HttpClient httpClient, 
        ILogger<SpotifyDataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<GetPlaylistResponse>> GetPlaylistAsync(
        GetPlaylistRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Get)
            .WithRequestUrl($"v1/playlists/{request.PlaylistId}")
            .WithAuthorizationTokenHeader(request.AccessToken)
            .Build();

        return await ExecuteHttpRequestAsync<GetPlaylistResponse>(httpRequestMessage, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteHttpRequestAsync<TResponse>(
        HttpRequestMessage requestMessage,
        CancellationToken cancellationToken = default) where TResponse : class
    {
        try
        {
            var httpResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var stringContentResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            return Result<TResponse>.Success(
                JsonSerializer.Deserialize<TResponse>(stringContentResponse)
                ?? throw new ArgumentNullException("HttpResponse", "Unable to deserialize Http Response Body"));
        }
        catch (HttpRequestException hre) when (hre.StatusCode == HttpStatusCode.Forbidden)
        {
            _logger.LogError(hre.Message);
            return Result<TResponse>.Failure(HttpStatusCode.Forbidden.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result<TResponse>.Failure("There is an issue with your request, please verify the logs.");
        }
    }
}
