using AutoMapper;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.Likes;
using SpeakingShorts.WebApi.Models.Likes;

namespace SpeakingShorts.WebApi.ApiService.Likes
{
    public class LikeApiService(ILikeService likeService, IMapper mapper) : ILikeApiService
    {
        public async ValueTask<LikeViewModel> CreateAsync(LikeCreateModel model)
        {
            var like = mapper.Map<Like>(model);
            var createdLike = await likeService.CreateAsync(like);
            return mapper.Map<LikeViewModel>(createdLike);
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            return await likeService.DeleteAsync(id);
        }

        public async ValueTask<LikeViewModel> GetAsync(long id)
        {
            var like = await likeService.GetAsync(id);
            return mapper.Map<LikeViewModel>(like);
        }

        public async ValueTask<IEnumerable<LikeViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var likes = await likeService.GetAllAsync(@params, filter, search);
            return mapper.Map<IEnumerable<LikeViewModel>>(likes);
        }

        public async ValueTask<IEnumerable<LikeViewModel>> GetAllAsync()
        {
            var likes = await likeService.GetAllAsync();
            return mapper.Map<IEnumerable<LikeViewModel>>(likes);
        }

        public async ValueTask<IEnumerable<LikeViewModel>> GetByContentIdAsync(long contentId)
        {
            var likes = await likeService.GetByContentIdAsync(contentId);
            return mapper.Map<IEnumerable<LikeViewModel>>(likes);
        }

        public async ValueTask<bool> ToggleLikeAsync(long contentId)
        {
            return await likeService.ToggleLikeAsync(contentId);
        }
    }
}
