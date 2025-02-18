using FilmModels;
using FilmServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmControllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistController : ControllerBase
{
    private readonly ArtistService _artistService;

    public ArtistController(ArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpPost]
    [Authorize("create:artist")]
    public async Task<ActionResult<Artist>> Create(Artist artist)
    {
        await _artistService.CreateAsync(artist);
        return Ok(artist);
    }

    [HttpGet("{id:length(24)}")]
    [Authorize("get:artist:specific")]
    public async Task<ActionResult<Artist>> GetById(string id)
    {
        var artist = await _artistService.GetArtistWithFilmsAsync(id);
        if (artist == null)
            return NotFound();
        return Ok(artist);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize("update:artist:specific")]
    public async Task<IActionResult> UpdateArtist(string id, [FromBody] Artist updatedArtist)
    {
        if (id != updatedArtist.Id)
            return BadRequest("Artist ID mismatch");

        var artist = await _artistService.GetArtistByIdAsync(id);
        if (artist == null)
            return NotFound();

        await _artistService.UpdateArtistAsync(id, updatedArtist);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Artist>>> Get()
    {
        var artists = await _artistService.GetAllArtistsWithFilmsAsync();
        if (artists.Count() == 0)
            return NotFound();
        return Ok(artists);
    }

    [HttpPost("{artistId:length(24)}/addFilm/{filmId:length(24)}")]
    [Authorize("add:artist:film")]
    public async Task<IActionResult> AddFilmToArtist(string artistId, string filmId)
    {
        await _artistService.AddFilmToArtist(artistId, filmId);
        return Ok(new { Message = "Film added to artist successfully" });
    }

    [HttpDelete("{artistId:length(24)}/deleteFilm/{filmId:length(24)}")]
    [Authorize("delete:artist:film")]
    public async Task<IActionResult> DeleteFilmToArtist(string artistId, string filmId)
    {
        await _artistService.DeleteFilmToArtist(artistId, filmId);
        return Ok(new { Message = "Film delete from artist successfully" });
    }

    [HttpDelete("{artistId:length(24)}")]
    [Authorize("delete:artist")]
    public async Task<IActionResult> DeleteArtist(string artistId)
    {
        var artist = await _artistService.GetArtistByIdAsync(artistId);
        if (artist == null)
        {
            return NotFound();
        }

        var result = await _artistService.DeleteArtistAsync(artistId);
        if (!result)
        {
            return StatusCode(500, "An error occurred while deleting the artist.");
        }

        return NoContent();
    }
}