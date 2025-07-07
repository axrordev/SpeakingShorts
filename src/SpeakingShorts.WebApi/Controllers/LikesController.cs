using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.ApiService.Likes;
using SpeakingShorts.WebApi.Models.Likes;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;
using Microsoft.AspNetCore.Authorization;

namespace SpeakingShorts.WebApi.Controllers;


public class LikesController(ILikeApiService likeApiService) : BaseController
{
    [Authorize]
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] LikeCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.CreateAsync(model)
        });

    [Authorize]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.GetAllAsync()
        });

    [HttpGet("by-content/{contentId:long}")]
    public async ValueTask<IActionResult> GetByContentIdAsync([FromRoute] long contentId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.GetByContentIdAsync(contentId)
        });

    [Authorize]
    [HttpPost("toggle/{contentId:long}")]
    public async ValueTask<IActionResult> ToggleLikeAsync([FromRoute] long contentId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await likeApiService.ToggleLikeAsync(contentId)
        });
} 