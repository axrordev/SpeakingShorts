using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.Stories;

public class StoryService(IUnitOfWork unitOfWork) : IStoryService
{
    public async ValueTask<Story> CreateAsync(Story story)
    {
        // DB'ga yozish logikasi shu yerda bo'ladi
        throw new NotImplementedException();
    }

    public async ValueTask<Story> ModifyAsync(long id, Story story)
    {
        // DB'da yangilash logikasi
        throw new NotImplementedException();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        // DB va storage'dan o'chirish logikasi
        throw new NotImplementedException();
    }

    public async ValueTask<Story> GetAsync(long id)
    {
        // DB'dan olish logikasi
        throw new NotImplementedException();
    }

    public async ValueTask<IEnumerable<Story>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var stories = unitOfWork.StoryRepository.Select(isTracking: false);

        if (!string.IsNullOrWhiteSpace(search))
            stories = stories.Where(s => s.Title.ToLower().Contains(search.ToLower()));

        return await stories.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<Story>> GetAllAsync()
    {
        // DB'dan barcha storylarni olish logikasi
        throw new NotImplementedException();
    }
} 