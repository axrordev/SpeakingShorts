using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.Models.UserCards;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.ApiService.UserCards;

namespace SpeakingShorts.WebApi.Controllers;


public class UserCardsController(IUserCardApiService userCardApiService) : BaseController
{
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] UserCardCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.CreateAsync(model)
        });

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] UserCardModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.ModifyAsync(id, model)
        });

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.GetAllAsync()
        });

    [HttpGet("by-user/{userId:long}")]
    public async ValueTask<IActionResult> GetByUserIdAsync([FromRoute] long userId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.GetByUserIdAsync(userId)
        });

    [HttpGet("by-card/{cardId:long}")]
    public async ValueTask<IActionResult> GetByCardIdAsync([FromRoute] long cardId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await userCardApiService.GetByCardIdAsync(cardId)
        });
} 