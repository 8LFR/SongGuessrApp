using Microsoft.AspNetCore.Mvc;
using SongGuessr.Shared;
using SongGuessr.Shared.Playlists;

namespace SongGuessr.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistsService _playlistsService;

    public PlaylistsController(IPlaylistsService playlistsService)
    {
        _playlistsService = playlistsService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAsync(
        [FromBody] GetPlaylistRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _playlistsService.GetPlaylistAsync(request, cancellationToken);

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
