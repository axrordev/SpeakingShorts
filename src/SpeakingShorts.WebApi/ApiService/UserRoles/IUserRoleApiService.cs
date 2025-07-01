
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.UserRoles;

namespace SpeakingShorts.WebApi.ApiService.UserRoles;

public interface IUserRoleApiService
{
    ValueTask<UserRoleViewModel> CreateAsync(UserRoleCreateModel createModel);
    ValueTask<UserRoleViewModel> UpdateAsync(long id, UserRoleUpdateModel updateModel);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserRoleViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<UserRoleViewModel>> GetAllAsync(
        PaginationParams @params,
        Filter filter,
        string search = null);
}
