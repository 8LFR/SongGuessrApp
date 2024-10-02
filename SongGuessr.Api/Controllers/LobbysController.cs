using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Lobbys;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LobbysController(ILobbysService lobbysService) : ControllerBase
{
    private readonly ILobbysService _lobbysService = lobbysService;

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateLobbyRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _lobbysService.CreateLobbyAsync(request, cancellationToken);

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

    [HttpPost("get")]
    public async Task<IActionResult> GetAsync(
        [FromBody] GetLobbyRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _lobbysService.GetLobbyAsync(request, cancellationToken);

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

    [HttpPost("get-all")]
    public async Task<IActionResult> GetAllAsync(
        [FromBody] GetAllLobbysRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _lobbysService.GetAllLobbysAsync(request, cancellationToken);

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

    [HttpPost("join")]
    public async Task<IActionResult> JoinAsync(
        [FromBody] JoinLobbyRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _lobbysService.JoinLobbyAsync(request, cancellationToken);

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
