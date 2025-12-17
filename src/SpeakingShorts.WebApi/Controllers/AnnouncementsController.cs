using Microsoft.AspNetCore.Mvc;
using SpeakingShorts.WebApi.ApiService.Announcements;
using SpeakingShorts.WebApi.Models.Announcements;
using SpeakingShorts.WebApi.Models.Commons;
using SpeakingShorts.Service.Configurations;
using Microsoft.AspNetCore.Authorization;

namespace SpeakingShorts.WebApi.Controllers;

public class AnnouncementsController(IAnnouncementApiService announcementApiService) : BaseController
{
    [Authorize(Roles = "admin, superadmin")]
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] AnnouncementCreateModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.CreateAsync(model)
        });

    [Authorize(Roles = "admin, superadmin")]
    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] AnnouncementModifyModel model)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.ModifyAsync(id, model)
        });

    [Authorize(Roles = "admin, superadmin")]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.DeleteAsync(id)
        });

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> GetAsync([FromRoute] long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.GetAsync(id)
        });

    [HttpGet]
    public async ValueTask<IActionResult> GetListAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.GetAllAsync(@params, filter, search)
        });

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await announcementApiService.GetAllAsync()
        });
} 