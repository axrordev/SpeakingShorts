using SpeakingShorts.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.UserCards;

public interface IUserCardService
{
    ValueTask<UserCard> CreateAsync(UserCard userCard);
    ValueTask<UserCard> ModifyAsync(long id, UserCard userCard);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserCard> GetAsync(long id);
    ValueTask<IEnumerable<UserCard>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<UserCard>> GetAllAsync();
    ValueTask<IEnumerable<UserCard>> GetByUserIdAsync(long userId);
    ValueTask<UserCard> GetByCardIdAsync(long cardId);
} 