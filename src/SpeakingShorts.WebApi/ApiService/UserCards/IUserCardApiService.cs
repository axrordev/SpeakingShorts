using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.UserCards;

namespace SpeakingShorts.WebApi.ApiService.UserCards
{
    // Interface
    public interface IUserCardApiService
    {
        ValueTask<UserCardViewModel> CreateAsync(UserCardCreateModel model);
        ValueTask<UserCardViewModel> ModifyAsync(long id, UserCardModifyModel model);
        ValueTask<bool> DeleteAsync(long id);
        ValueTask<UserCardViewModel> GetAsync(long id);
        ValueTask<IEnumerable<UserCardViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
        ValueTask<IEnumerable<UserCardViewModel>> GetAllAsync();
        ValueTask<IEnumerable<UserCardViewModel>> GetByUserIdAsync(long userId);
        ValueTask<UserCardViewModel> GetByCardIdAsync(long cardId);
    }
}