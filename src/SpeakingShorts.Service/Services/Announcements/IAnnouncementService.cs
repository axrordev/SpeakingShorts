using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.Announcements;

public interface IAnnouncementService
{
    ValueTask<Announcement> CreateAsync(Announcement announcement);
    ValueTask<Announcement> ModifyAsync(long id, Announcement announcement);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<Announcement> GetAsync(long id);
    ValueTask<IEnumerable<Announcement>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<Announcement>> GetAllAsync();
} 