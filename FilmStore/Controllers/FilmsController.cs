using FilmStore.Services;
using FilmStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FilmStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly FilmsService _filmsService;

    public FilmsController(FilmsService filmsService) => _filmsService = filmsService;

    [HttpGet]
    public async Task<List<Film>> Get() 
		=> await _filmsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    [Authorize("get:film:specific")]
    public async Task<ActionResult<Film>> Get(string id)
    {
        var film = await _filmsService.GetAsync(id);
        if (film is null)
            return NotFound();
        return film;
    }

    [HttpPost]
    [Authorize("create:film")]
    public async Task<IActionResult> Post(Film newFilm)
    {
        await _filmsService.CreateAsync(newFilm);
        return CreatedAtAction(nameof(Get), new { id = newFilm.Id }, newFilm);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize("update:film")]
    public async Task<IActionResult> Update(string id, Film updatedFilm)
    {
        var film = await _filmsService.GetAsync(id);

        if (film is null)
            return NotFound();

        updatedFilm.Id = film.Id;
        await _filmsService.UpdateAsync(id, updatedFilm);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize("delete:film")]
    public async Task<IActionResult> Delete(string id)
    {
        var film = await _filmsService.GetAsync(id);

        if (film is null)
            return NotFound();

        await _filmsService.RemoveAsync(id);
        return NoContent();
    }
}