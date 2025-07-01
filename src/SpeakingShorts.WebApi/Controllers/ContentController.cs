using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Commons;

namespace SpeakingShorts.WebApi.Controllers;

public class ContentsController(IContentApiService contentApiService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(
         IFormFile file, 
        [FromQuery] ContentType type, 
        [FromQuery] string title)
    {
        var content = await contentApiService.CreateAndProcessAsync(file, type, title);
        return Accepted(content); // 202 Accepted, chunki qayta ishlash fonda ketishi mumkin
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await contentApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([
        FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await contentApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await contentApiService.GetAllAsync()
        });

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] ContentModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await contentApiService.ModifyAsync(id, model)
        });

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
    {
        await contentApiService.DeleteAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success"
        });
    }
}





