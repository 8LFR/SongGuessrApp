namespace SongGuessr.Shared.Authentication;

public interface IAuthenticationService
{
    Task<Result<GetSpotifyAccessTokenResponse>> GetSpotifyAccessTokenAsync();
}
