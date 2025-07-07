using AutoMapper;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.Stories;
using SpeakingShorts.WebApi.Models.Stories;


namespace SpeakingShorts.WebApi.ApiService.Stories
{
public class StoryApiService(IStoryService storyService, IMapper mapper) : IStoryApiService
    {

        public async ValueTask<StoryViewModel> CreateAsync(StoryCreateModel model)
        {
            var story = mapper.Map<Story>(model);
            var createdStory = await storyService.CreateAsync(story);
            return mapper.Map<StoryViewModel>(createdStory);
        }

        public async ValueTask<StoryViewModel> ModifyAsync(long id, StoryModifyModel model)
        {
            var story = mapper.Map<Story>(model);
            var modifiedStory = await storyService.ModifyAsync(id, story);
            return mapper.Map<StoryViewModel>(modifiedStory);
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            return await storyService.DeleteAsync(id);
        }

        public async ValueTask<StoryViewModel> GetAsync(long id)
        {
            var story = await storyService.GetAsync(id);
            return mapper.Map<StoryViewModel>(story);
        }

        public async ValueTask<IEnumerable<StoryViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var stories = await storyService.GetAllAsync(@params, filter, search);
            return mapper.Map<IEnumerable<StoryViewModel>>(stories);
        }

        public async ValueTask<IEnumerable<StoryViewModel>> GetAllAsync()
        {
            var stories = await storyService.GetAllAsync();
            return mapper.Map<IEnumerable<StoryViewModel>>(stories);
        }
    }
}
