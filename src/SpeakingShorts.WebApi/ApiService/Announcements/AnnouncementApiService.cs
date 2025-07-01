using AutoMapper;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.Announcements;
using SpeakingShorts.WebApi.ApiService.Announcements;
using SpeakingShorts.WebApi.Models.Announcements;

public class AnnouncementApiService(IMapper mapper, IAnnouncementService announcementService ) : IAnnouncementApiService
{
    public async ValueTask<AnnouncementViewModel> CreateAsync(AnnouncementCreateModel model)
    {
        var announcement = mapper.Map<Announcement>(model);
        var createdAnnouncement = await announcementService.CreateAsync(announcement);
        return mapper.Map<AnnouncementViewModel>(createdAnnouncement);
    }

    public async ValueTask<AnnouncementViewModel> ModifyAsync(long id, AnnouncementModifyModel model)
    {
        var announcement = mapper.Map<Announcement>(model);
        var modifiedAnnouncement = await announcementService.ModifyAsync(id, announcement);
        return mapper.Map<AnnouncementViewModel>(modifiedAnnouncement);
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        return await announcementService.DeleteAsync(id);
    }

    public async ValueTask<AnnouncementViewModel> GetAsync(long id)
    {
        var announcement = await announcementService.GetAsync(id);
        return mapper.Map<AnnouncementViewModel>(announcement);
    }

    public async ValueTask<IEnumerable<AnnouncementViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var announcements = await announcementService.GetAllAsync(@params, filter, search);
        return mapper.Map<IEnumerable<AnnouncementViewModel>>(announcements);
    }

    public async ValueTask<IEnumerable<AnnouncementViewModel>> GetAllAsync()
    {
        var announcements = await announcementService.GetAllAsync();
        return mapper.Map<IEnumerable<AnnouncementViewModel>>(announcements);
    }

}