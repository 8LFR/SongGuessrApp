namespace SongGuessr.Shared.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISpotifyAuthService _spotifyAuthService;

    public AuthenticationService(ISpotifyAuthService spotifyAuthService)
    {
        _spotifyAuthService = spotifyAuthService;
    }

    public async Task<Result<GetSpotifyAccessTokenResponse>> GetSpotifyAccessTokenAsync()
    {
        return await _spotifyAuthService.GetSpotifyAccessTokenAsync();
    }
}
