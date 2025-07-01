using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Announcements;

namespace SpeakingShorts.WebApi.ApiService.Announcements;

public interface IAnnouncementApiService
{
    ValueTask<AnnouncementViewModel> CreateAsync(AnnouncementCreateModel model);
    ValueTask<AnnouncementViewModel> ModifyAsync(long id, AnnouncementModifyModel model);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<AnnouncementViewModel> GetAsync(long id);
    ValueTask<IEnumerable<AnnouncementViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<AnnouncementViewModel>> GetAllAsync();
}