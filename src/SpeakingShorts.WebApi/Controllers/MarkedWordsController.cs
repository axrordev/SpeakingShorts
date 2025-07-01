using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.Models.MarkedWords;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.ApiService.MarkedWords;

namespace SpeakingShorts.WebApi.Controllers;


public class MarkedWordsController(IMarkedWordApiService markedWordApiService) : BaseController
{
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] MarkedWordCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.CreateAsync(model)
        });

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] MarkedWordModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.ModifyAsync(id, model)
        });

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.GetAllAsync()
        });

    [HttpGet("by-story/{storyId:long}")]
    public async ValueTask<IActionResult> GetByStoryIdAsync([FromRoute] long storyId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await markedWordApiService.GetByStoryIdAsync(storyId)
        });
} 