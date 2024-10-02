using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Game;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartGameAsync(
        StartGameRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _gameService.StartGameAsync(request, cancellationToken);

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
