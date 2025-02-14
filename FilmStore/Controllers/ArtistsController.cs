using ArtistStore.Services;
using FilmStore.Models;
using FilmStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
	private readonly ArtistRepository _artistRepository;

	public ArtistsController(ArtistRepository artistRepository) 
		=> _artistRepository = artistRepository;

	[HttpPost]
    [Authorize("create:artist")]
    public async Task<IActionResult> Create(Artist artist)
	{
		await _artistRepository.CreateAsync(artist);
		return Ok(artist);
	}	

	[HttpGet("{id:length(24)}")]
    [Authorize("get:artist:specific")]
    public async Task<IActionResult> GetById(string id)
	{
		var artist = await _artistRepository.GetArtistWithFilmsAsync(id);
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

        var artist = await _artistRepository.GetArtistByIdAsync(id);
        if (artist == null)
            return NotFound();

        await _artistRepository.UpdateArtistAsync(id, updatedArtist);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
	{
		var artists = await _artistRepository.GetAllArtistsWithFilmsAsync();
		if (artists.Count() == 0) 
			return NotFound();
		return Ok(artists);
	}

	[HttpPost("{artistId:length(24)}/addFilm/{filmId:length(24)}")]
    [Authorize("add:artist:film")]
    public async Task<IActionResult> AddFilmToArtist(string artistId, string filmId)
	{
		await _artistRepository.AddFilmToArtist(artistId, filmId);
		return Ok(new { Message = "Film added to artist successfully" });
	}

	[HttpDelete("{artistId:length(24)}/deleteFilm/{filmId:length(24)}")]
    [Authorize("delete:artist:film")]
    public async Task<IActionResult> DeleteFilmToArtist(string artistId, string filmId)
	{
		await _artistRepository.DeleteFilmToArtist(artistId, filmId);
		return Ok(new { Message = "Film delete from artist successfully" });
	}

	[HttpDelete("{artistId:length(24}")]
	[Authorize("delete:artist")]
    public async Task<IActionResult> DeleteArtist(string id)
    {
        var artist = await _artistRepository.GetArtistByIdAsync(id);
        if (artist == null)
        {
            return NotFound();
        }

        var result = await _artistRepository.DeleteArtistAsync(id);
        if (!result)
        {
            return StatusCode(500, "An error occurred while deleting the artist.");
        }

        return NoContent();
    }
}