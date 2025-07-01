using AutoMapper;
using Microsoft.AspNetCore.Http;
using SpeakingShorts.Service.Services.Contents;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.WebApi.ApiService.Contents
{
    public class ContentApiService(IContentService contentService, IMapper mapper) : IContentApiService
    {
        public async Task<ContentViewModel> CreateAndProcessAsync(IFormFile file, ContentType type, string title)
        {
            var content = await contentService.CreateAndProcessAsync(file, type, title);
            return mapper.Map<ContentViewModel>(content);
        }

        public async ValueTask<ContentViewModel> GetAsync(long id)
        {
            var content = await contentService.GetAsync(id);
            return mapper.Map<ContentViewModel>(content);
        }

        public async ValueTask<IEnumerable<ContentViewModel>> GetAllAsync()
        {
            var contents = await contentService.GetAllAsync();
            return mapper.Map<IEnumerable<ContentViewModel>>(contents);
        }

        public async ValueTask<IEnumerable<ContentViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var contents = await contentService.GetAllAsync(@params, filter, search);
            return mapper.Map<IEnumerable<ContentViewModel>>(contents);
        }

        public async ValueTask<ContentViewModel> ModifyAsync(long id, ContentModifyModel model)
        {
            var content = mapper.Map<Content>(model);
            var modifiedContent = await contentService.ModifyAsync(id, content);
            return mapper.Map<ContentViewModel>(modifiedContent);
        }

        public async ValueTask DeleteAsync(long id)
        {
            await contentService.DeleteAsync(id);
        }
    }


}