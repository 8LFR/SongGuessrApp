using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Tracks;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ITracksService _tracksService;

    public TracksController(ITracksService tracksService)
    {
        _tracksService = tracksService;
    }

    [HttpPost("get-random")]
    public async Task<IActionResult> GetRandomAsync(
    [FromBody] GetRandomTrackRequest request,
    CancellationToken cancellationToken = default)
    {
        var result = await _tracksService.GetRandomTrackAsync(request, cancellationToken);

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
