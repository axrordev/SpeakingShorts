
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.ApiService.Users;

public interface IUserApiService
{
    ValueTask<UserViewModel> ModifyAsync(long id, UserUpdateModel updateModel);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserViewModel> GetAsync(long id);
    ValueTask<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<UserViewModel> ChangePasswordAsync(string oldPasword, string newPassword, string confirmPassword);
    ValueTask<UserViewModel> ChangeRoleAsync(long userId, long roleId);
}