using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.UserRoles;

public interface IUserRoleService
{
    ValueTask<UserRole> CreateAsync(UserRole userRole);
    ValueTask<UserRole> ModifyAsync(long id, UserRole userRole);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserRole> GetByIdAsync(long id);
    ValueTask<IEnumerable<UserRole>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<UserRole>> GetAllAsync();
}