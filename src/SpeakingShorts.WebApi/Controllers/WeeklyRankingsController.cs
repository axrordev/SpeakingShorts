using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.ApiService.WeeklyRankings;
using SpeakingShorts.WebApi.Models.WeeklyRankings;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.WebApi.Controllers;


public class WeeklyRankingsController(IWeeklyRankingApiService weeklyRankingApiService) : BaseController
{
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetAllAsync()
        });

    [HttpGet("by-user/{userId:long}")]
    public async ValueTask<IActionResult> GetByUserIdAsync([FromRoute] long userId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetByUserIdAsync(userId)
        });

    [HttpGet("current-week")]
    public async ValueTask<IActionResult> GetCurrentWeekRankingAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetCurrentWeekRankingAsync()
        });

    [HttpGet("last-week")]
    public async ValueTask<IActionResult> GetLastWeekRankingAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetLastWeekRankingAsync()
        });

    [HttpGet("by-week/{weekNumber:int}")]
    public async ValueTask<IActionResult> GetByWeekNumberAsync([FromRoute] int weekNumber)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await weeklyRankingApiService.GetByWeekNumberAsync(weekNumber)
        });
} 