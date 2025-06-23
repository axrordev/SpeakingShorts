using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.Service.Services.BackblazeServices;
using SpeakingShorts.Service.Services.Contents;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;


namespace SpeakingShorts.WebApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
         IFormFile file, 
        [FromQuery] ContentType type, 
        [FromQuery] string title, 
        [FromQuery] long? backgroundMusicId)
    {
        var content = await _contentService.CreateAndProcessAsync(file, type, title, backgroundMusicId);
        return Accepted(content); // 202 Accepted, chunki qayta ishlash fonda ketishi mumkin
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var content = await _contentService.GetAsync(id);
        return Ok(content);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contents = await _contentService.GetAllAsync();
        return Ok(contents);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _contentService.DeleteAsync(id);
        return NoContent();
    }
}

}
