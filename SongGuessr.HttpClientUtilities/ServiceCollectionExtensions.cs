using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SongGuessr.Shared;

namespace SongGuessr.HttpClientUtilities;

public static class ServiceCollectionExtensions
{
    public static void AddSpotifyHttpClientServices(this IServiceCollection services, SpotifySettings spotifySettings)
    {
        services
            .AddSingleton(spotifySettings)
            .AddHttpClient<ISpotifyDataService, SpotifyDataService>(

                client =>
                {
                    Console.WriteLine($"BaseUri: {spotifySettings.BaseUri}");
                    Console.WriteLine($"TimeoutInSeconds: {spotifySettings.TimeoutInSeconds}");

                    client.BaseAddress = spotifySettings.BaseUri ??
                                         throw new ArgumentNullException(nameof(spotifySettings.BaseUri),
                                            "Missing Spotify Http Client Base Uri configuration");
                    client.Timeout = TimeSpan.FromSeconds(spotifySettings.TimeoutInSeconds ??
                                                          throw new ArgumentNullException(
                                                              nameof(spotifySettings.TimeoutInSeconds), ""));
                })
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    }
}
