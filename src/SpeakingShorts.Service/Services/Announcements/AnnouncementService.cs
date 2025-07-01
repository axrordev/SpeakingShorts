using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;

namespace SpeakingShorts.Service.Services.Announcements;

public class AnnouncementService(IUnitOfWork unitOfWork) : IAnnouncementService
{
    public async ValueTask<Announcement> CreateAsync(Announcement announcement)
    {
        var createdAnnouncement = await unitOfWork.AnnouncementRepository.InsertAsync(announcement);
        createdAnnouncement.CreatedAt = DateTime.UtcNow;
        await unitOfWork.SaveAsync();
        return createdAnnouncement;
    }

    public async ValueTask<Announcement> ModifyAsync(long id, Announcement announcement)
    {
        var existAnnouncement = await unitOfWork.AnnouncementRepository.SelectAsync(a => a.Id == id)
            ?? throw new NotFoundException("Announcement not found");

        existAnnouncement.Title = announcement.Title;
        existAnnouncement.Message = announcement.Message;
        existAnnouncement.ExpireDate = announcement.ExpireDate;
        existAnnouncement.IsActive = announcement.IsActive;
        existAnnouncement.UpdatedAt = DateTime.UtcNow;

        var updatedAnnouncement = await unitOfWork.AnnouncementRepository.UpdateAsync(existAnnouncement);
        await unitOfWork.SaveAsync();

        return updatedAnnouncement;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existAnnouncement = await unitOfWork.AnnouncementRepository.SelectAsync(a => a.Id == id)
            ?? throw new NotFoundException("Announcement not found");

        await unitOfWork.AnnouncementRepository.DeleteAsync(existAnnouncement);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<Announcement> GetAsync(long id)
    {
        return await unitOfWork.AnnouncementRepository
            .SelectAsync(expression: announcement => announcement.Id == id)
            ?? throw new NotFoundException("Announcement not found");
    }

    public async ValueTask<IEnumerable<Announcement>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var announcements = unitOfWork.AnnouncementRepository.Select(isTracking: false);

        if (!string.IsNullOrWhiteSpace(search))
            announcements = announcements.Where(a => 
                a.Title.ToLower().Contains(search.ToLower()));

        return await announcements.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<Announcement>> GetAllAsync()
    {
        return await unitOfWork.AnnouncementRepository
            .Select(isTracking: false)
            .ToListAsync();
    }
} 