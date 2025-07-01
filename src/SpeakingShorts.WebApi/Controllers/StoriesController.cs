using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.ApiService.Stories;
using SpeakingShorts.WebApi.Models.Stories;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.WebApi.Controllers;

public class StoriesController(IStoryApiService storyApiService) : BaseController
{
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] StoryCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.CreateAsync(model)
        });

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] StoryModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.ModifyAsync(id, model)
        });

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await storyApiService.GetAllAsync()
        });
} 