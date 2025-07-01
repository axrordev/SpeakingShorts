using AutoMapper;
using SpeakingShorts.Service.Services.UserCards;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.UserCards;

namespace SpeakingShorts.WebApi.ApiService.UserCards
{
    public class UserCardApiService(IUserCardService userCardService, IMapper mapper) : IUserCardApiService
    {
        public async ValueTask<UserCardViewModel> CreateAsync(UserCardCreateModel model)
        {
            var userCard = mapper.Map<UserCard>(model);
            var createdUserCard = await userCardService.CreateAsync(userCard);
            return mapper.Map<UserCardViewModel>(createdUserCard);
        }

        public async ValueTask<UserCardViewModel> ModifyAsync(long id, UserCardModifyModel model)
        {
            var userCard = mapper.Map<UserCard>(model);
            var modifiedUserCard = await userCardService.ModifyAsync(id, userCard);
            return mapper.Map<UserCardViewModel>(modifiedUserCard);
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            return await userCardService.DeleteAsync(id);
        }

        public async ValueTask<UserCardViewModel> GetAsync(long id)
        {
            var userCard = await userCardService.GetAsync(id);
            return mapper.Map<UserCardViewModel>(userCard);
        }

        public async ValueTask<IEnumerable<UserCardViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var userCards = await userCardService.GetAllAsync(@params, filter, search);
            return mapper.Map<IEnumerable<UserCardViewModel>>(userCards);
        }

        public async ValueTask<IEnumerable<UserCardViewModel>> GetAllAsync()
        {
            var userCards = await userCardService.GetAllAsync();
            return mapper.Map<IEnumerable<UserCardViewModel>>(userCards);
        }

        public async ValueTask<IEnumerable<UserCardViewModel>> GetByUserIdAsync(long userId)
        {
            var userCards = await userCardService.GetByUserIdAsync(userId);
            return mapper.Map<IEnumerable<UserCardViewModel>>(userCards);
        }

        public async ValueTask<UserCardViewModel> GetByCardIdAsync(long cardId)
        {
            var userCard = await userCardService.GetByCardIdAsync(cardId);
            return mapper.Map<UserCardViewModel>(userCard);
        }
    }
}