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
            var userId = HttpContextHelper.GetUserId;
            if (userId == null || userId == 0)
            {
                // Login qilmagan foydalanuvchi uchun xabar qaytarish
                throw new UnauthorizedAccessException("Please log in !");
            }
            story.CreatedById = userId;

            var createdStory = await unitOfWork.StoryRepository.InsertAsync(story);
            await unitOfWork.SaveAsync();
            return createdStory;
        }

        public async ValueTask<Story> ModifyAsync(long id, Story story)
        {
            var existStory = await unitOfWork.StoryRepository.SelectAsync(s => s.Id == id)
                ?? throw new NotFoundException("Story not found");

            existStory.Title = story.Title;
            existStory.Text = story.Text;
            existStory.UpdatedById = HttpContextHelper.GetUserId;
            existStory.UpdatedAt = DateTime.UtcNow;

            var updatedStory = await unitOfWork.StoryRepository.UpdateAsync(existStory);
            await unitOfWork.SaveAsync();

            return updatedStory;
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            var existStory = await unitOfWork.StoryRepository.SelectAsync(s => s.Id == id)
                ?? throw new NotFoundException("Story not found");

            existStory.DeletedById = HttpContextHelper.GetUserId;
            await unitOfWork.StoryRepository.DeleteAsync(existStory);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<Story> GetAsync(long id)
        {
            return await unitOfWork.StoryRepository
                .SelectAsync(expression: story => story.Id == id, includes: ["MarkedWords"])
                ?? throw new NotFoundException("Story not found");
        }

        public async ValueTask<IEnumerable<Story>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var stories = unitOfWork.StoryRepository.Select(isTracking: false, includes: ["MarkedWords"]);

            if (!string.IsNullOrWhiteSpace(search))
                stories = stories.Where(s => s.Title.ToLower().Contains(search.ToLower()));

            return await stories.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
        }

        public async ValueTask<IEnumerable<Story>> GetAllAsync()
        {
            return await unitOfWork.StoryRepository
                .Select(isTracking: false, includes: ["MarkedWords"])
                .ToListAsync();
        }
    }