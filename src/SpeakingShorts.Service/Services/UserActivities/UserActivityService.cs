using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.UserActivities;

public class UserActivityService(IUnitOfWork unitOfWork) : IUserActivityService
{
    public async ValueTask<UserActivity> CreateAsync(UserActivity userActivity)
    {
        userActivity.UserId = HttpContextHelper.GetUserId;
        userActivity.CreatedById = HttpContextHelper.GetUserId;
        userActivity.UploadDate = DateTime.UtcNow;

        var createdUserActivity = await unitOfWork.UserActivityRepository.InsertAsync(userActivity);
        await unitOfWork.SaveAsync();
        return createdUserActivity;
    }

    public async ValueTask<UserActivity> ModifyAsync(long id, UserActivity userActivity)
    {
        var existUserActivity = await unitOfWork.UserActivityRepository.SelectAsync(u => u.Id == id)
            ?? throw new NotFoundException("User activity not found");

        if (existUserActivity.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to modify this user activity");

        existUserActivity.IsContentDeleted = userActivity.IsContentDeleted;
        existUserActivity.UpdatedById = HttpContextHelper.GetUserId;
        existUserActivity.UpdatedAt = DateTime.UtcNow;

        var updatedUserActivity = await unitOfWork.UserActivityRepository.UpdateAsync(existUserActivity);
        await unitOfWork.SaveAsync();

        return updatedUserActivity;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existUserActivity = await unitOfWork.UserActivityRepository.SelectAsync(u => u.Id == id)
            ?? throw new NotFoundException("User activity not found");

        if (existUserActivity.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to delete this user activity");

        existUserActivity.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.UserActivityRepository.DeleteAsync(existUserActivity);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<UserActivity> GetAsync(long id)
    {
        return await unitOfWork.UserActivityRepository
            .SelectAsync(expression: userActivity => userActivity.Id == id, includes: ["User"])
            ?? throw new NotFoundException("User activity not found");
    }

    public async ValueTask<IEnumerable<UserActivity>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var userActivities = unitOfWork.UserActivityRepository.Select(isTracking: false, includes: ["User"]);

        return await userActivities.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<UserActivity>> GetAllAsync()
    {
        return await unitOfWork.UserActivityRepository
            .Select(isTracking: false, includes: ["User"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<UserActivity>> GetByUserIdAsync(long userId)
    {
        return await unitOfWork.UserActivityRepository
            .Select(expression: userActivity => userActivity.UserId == userId)
            .ToListAsync();
    }

    public async ValueTask<UserActivity> MarkContentAsDeletedAsync(long contentId)
    {
        var existUserActivity = await unitOfWork.UserActivityRepository
            .SelectAsync(u => u.UserId == HttpContextHelper.GetUserId)
            ?? throw new NotFoundException("User activity not found");

        existUserActivity.IsContentDeleted = true;
        existUserActivity.UpdatedById = HttpContextHelper.GetUserId;
        existUserActivity.UpdatedAt = DateTime.UtcNow;

        var updatedUserActivity = await unitOfWork.UserActivityRepository.UpdateAsync(existUserActivity);
        await unitOfWork.SaveAsync();

        return updatedUserActivity;
    }
} 