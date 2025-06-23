using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.UserRoles;

public class UserRoleService(IUnitOfWork unitOfWork) : IUserRoleService
{
    public async ValueTask<UserRole> CreateAsync(UserRole userRole)
    {
        var existRole = await unitOfWork.UserRoleRepository.SelectAsync(r => r.Name.ToLower() == userRole.Name.ToLower());
        if (existRole is not null)
        {
            throw new AlreadyExistException($"This role is already exist with this name | Name={userRole.Name}");
        }

        var createdRole = await unitOfWork.UserRoleRepository.InsertAsync(userRole);
        await unitOfWork.SaveAsync();
        return createdRole;
    }

    public async ValueTask<UserRole> ModifyAsync(long id, UserRole userRole)
    {
        var existRole = await unitOfWork.UserRoleRepository.SelectAsync(r => r.Id == id)
            ?? throw new NotFoundException("Role not found");

        var roleWithSameName = await unitOfWork.UserRoleRepository.SelectAsync(r => r.Name.ToLower() == userRole.Name.ToLower() && r.Id != id);
        if (roleWithSameName is not null)
        {
            throw new AlreadyExistException($"This role is already exist with this name | Name={userRole.Name}");
        }

        existRole.Name = userRole.Name;
        existRole.UpdatedById = HttpContextHelper.GetUserId;
        existRole.UpdatedAt = DateTime.UtcNow;

        var updatedRole = await unitOfWork.UserRoleRepository.UpdateAsync(existRole);
        await unitOfWork.SaveAsync();

        return updatedRole;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existRole = await unitOfWork.UserRoleRepository.SelectAsync(r => r.Id == id)
            ?? throw new NotFoundException("Role not found");

        existRole.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.UserRoleRepository.DeleteAsync(existRole);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<UserRole> GetAsync(long id)
    {
        return await unitOfWork.UserRoleRepository
            .SelectAsync(expression: role => role.Id == id)
            ?? throw new NotFoundException("Role not found");
    }

    public async ValueTask<IEnumerable<UserRole>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var roles = unitOfWork.UserRoleRepository.Select(isTracking: false);

        if (!string.IsNullOrWhiteSpace(search))
            roles = roles.Where(r => r.Name.ToLower().Contains(search.ToLower()));

        return await roles.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<UserRole>> GetAllAsync()
    {
        return await unitOfWork.UserRoleRepository
            .Select(isTracking: false)
            .ToListAsync();
    }

}
