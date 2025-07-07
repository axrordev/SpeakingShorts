using AutoMapper;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.UserRoles;
using SpeakingShorts.WebApi.Models.UserRoles;

namespace SpeakingShorts.WebApi.ApiService.UserRoles;

public class UserRoleApiService(IUserRoleService userRoleService, IMapper mapper) : IUserRoleApiService
{
    public async ValueTask<UserRoleViewModel> CreateAsync(UserRoleCreateModel createModel)
    {
        var createdUserRole = await userRoleService.CreateAsync(mapper.Map<UserRole>(createModel));
        return mapper.Map<UserRoleViewModel>(createdUserRole);
    }

    public async ValueTask<UserRoleViewModel> UpdateAsync(long id, UserRoleUpdateModel updateModel)
    {
        var updatedUserRole = await userRoleService.ModifyAsync(id, mapper.Map<UserRole>(updateModel));
        return mapper.Map<UserRoleViewModel>(updatedUserRole);
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        return await userRoleService.DeleteAsync(id);
    }

    public async ValueTask<IEnumerable<UserRoleViewModel>> GetAllAsync(
        PaginationParams @params,
        Filter filter,
        string search = null)
    {
        var result = await userRoleService.GetAllAsync(@params, filter, search);
        return mapper.Map<IEnumerable<UserRoleViewModel>>(result);
    }

    public async ValueTask<UserRoleViewModel> GetByIdAsync(long id)
    {
        var result = await userRoleService.GetByIdAsync(id);
        return mapper.Map<UserRoleViewModel>(result);
    }
}
