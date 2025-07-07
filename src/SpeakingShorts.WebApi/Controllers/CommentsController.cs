using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.ApiService.Comments;
using SpeakingShorts.WebApi.Models.Comments;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;
using Microsoft.AspNetCore.Authorization;

namespace SpeakingShorts.WebApi.Controllers;


public class CommentsController(ICommentApiService commentApiService) : BaseController
{
    [Authorize]
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] CommentCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.CreateAsync(model)
        });

    [Authorize]
    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] CommentModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.ModifyAsync(id, model)
        });

    [Authorize]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.GetAllAsync()
        });

    [HttpGet("by-content/{contentId:long}")]
    public async ValueTask<IActionResult> GetByContentIdAsync([FromRoute] long contentId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await commentApiService.GetByContentIdAsync(contentId)
        });
} 