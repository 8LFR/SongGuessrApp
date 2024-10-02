using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Players;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(IPlayersService playersService) : ControllerBase
{
    private readonly IPlayersService _playersService = playersService;

    [HttpPost("get-all")]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _playersService.GetAllPlayersAsync();

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

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePlayerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _playersService.CreatePlayerAsync(request, cancellationToken);

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
