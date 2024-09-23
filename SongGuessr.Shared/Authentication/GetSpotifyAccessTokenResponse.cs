using System.Text.Json.Serialization;

namespace SongGuessr.Shared.Authentication;

public record GetSpotifyAccessTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("expires_in")] int ExpiresIn);
