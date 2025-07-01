using AutoMapper;

using SpeakingShorts.Service.Services.MarkedWords;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.MarkedWords;

namespace SpeakingShorts.WebApi.ApiService.MarkedWords
{
    public class MarkedWordApiService(IMarkedWordService markedWordService, IMapper mapper) : IMarkedWordApiService
    {
        public async ValueTask<MarkedWordViewModel> CreateAsync(MarkedWordCreateModel model)
        {
            var markedWord = mapper.Map<MarkedWord>(model);
            var createdMarkedWord = await markedWordService.CreateAsync(markedWord);
            return mapper.Map<MarkedWordViewModel>(createdMarkedWord);
        }

        public async ValueTask<MarkedWordViewModel> ModifyAsync(long id, MarkedWordModifyModel model)
        {
            var markedWord = mapper.Map<MarkedWord>(model);
            var modifiedMarkedWord = await markedWordService.ModifyAsync(id, markedWord);
            return mapper.Map<MarkedWordViewModel>(modifiedMarkedWord);
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            return await markedWordService.DeleteAsync(id);
        }

        public async ValueTask<MarkedWordViewModel> GetAsync(long id)
        {
            var markedWord = await markedWordService.GetAsync(id);
            return mapper.Map<MarkedWordViewModel>(markedWord);
        }

        public async ValueTask<IEnumerable<MarkedWordViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var markedWords = await markedWordService.GetAllAsync(@params, filter, search);
            return mapper.Map<IEnumerable<MarkedWordViewModel>>(markedWords);
        }

        public async ValueTask<IEnumerable<MarkedWordViewModel>> GetAllAsync()
        {
            var markedWords = await markedWordService.GetAllAsync();
            return mapper.Map<IEnumerable<MarkedWordViewModel>>(markedWords);
        }

        public async ValueTask<IEnumerable<MarkedWordViewModel>> GetByStoryIdAsync(long storyId)
        {
            var markedWords = await markedWordService.GetByStoryIdAsync(storyId);
            return mapper.Map<IEnumerable<MarkedWordViewModel>>(markedWords);
        }
    }
}