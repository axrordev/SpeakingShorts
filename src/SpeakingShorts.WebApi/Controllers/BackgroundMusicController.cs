using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.Service.Services.BackgroundMusics;
using SpeakingShorts.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class BackgroundMusicController : ControllerBase
{
    private readonly IBackgroundMusicService _musicService;

    public BackgroundMusicController(IBackgroundMusicService musicService)
    {
        _musicService = musicService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, [FromForm] string title)
    {
        var music = await _musicService.CreateAsync(file, title);
        return Ok(new
        {
            music.Id,
            music.Title,
            music.FileUrl
        });
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var musics = await _musicService.GetAllAsync();
        var result = musics.Select(m => new
        {
            m.Id,
            m.Title,
            m.FileUrl
        });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var music = await _musicService.GetAsync(id);
        if (music == null) return NotFound();
        return Ok(new
        {
            music.Id,
            music.Title,
            music.FileUrl
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _musicService.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
} 