using SongGuessr.Shared.Authentication;

namespace SongGuessr.Shared;

public interface ISpotifyAuthService
{
    Task<Result<GetSpotifyAccessTokenResponse>> GetSpotifyAccessTokenAsync();
}
