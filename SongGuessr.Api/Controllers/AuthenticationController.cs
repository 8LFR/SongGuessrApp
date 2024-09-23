using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Authentication;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("get-token")]
    public async Task<IActionResult> GetSpotifyTokenAsync()
    {
        var result = await _authenticationService.GetSpotifyAccessTokenAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == KnownResultErrors.Forbidden)
        {
            return Forbid();
        }

        return BadRequest(result.Error);
    }
}
