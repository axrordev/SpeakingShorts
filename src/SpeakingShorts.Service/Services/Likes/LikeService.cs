using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.Likes;

public class LikeService(IUnitOfWork unitOfWork) : ILikeService
{
    public async ValueTask<Like> CreateAsync(Like like)
    {
        like.UserId = HttpContextHelper.GetUserId;
        like.CreatedById = HttpContextHelper.GetUserId;
        if (like.UserId == null || like.UserId == 0)
        {
            // Login qilmagan foydalanuvchi uchun xabar qaytarish
            throw new UnauthorizedAccessException("Please log in !");
        }
        var createdLike = await unitOfWork.LikeRepository.InsertAsync(like);
        await unitOfWork.SaveAsync();
        return createdLike;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existLike = await unitOfWork.LikeRepository.SelectAsync(l => l.Id == id)
            ?? throw new NotFoundException("Like not found");

        if (existLike.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to delete this like");

        existLike.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.LikeRepository.DeleteAsync(existLike);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<Like> GetAsync(long id)
    {
        return await unitOfWork.LikeRepository
            .SelectAsync(expression: like => like.Id == id, includes: ["User", "Content"])
            ?? throw new NotFoundException("Like not found");
    }

    public async ValueTask<IEnumerable<Like>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var likes = unitOfWork.LikeRepository.Select(isTracking: false, includes: ["User", "Content"]);

        return await likes.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<Like>> GetAllAsync()
    {
        return await unitOfWork.LikeRepository
            .Select(isTracking: false, includes: ["User", "Content"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<Like>> GetByContentIdAsync(long contentId)
    {
        return await unitOfWork.LikeRepository
            .Select(expression: like => like.ContentId == contentId, includes: ["User"])
            .ToListAsync();
    }

    public async ValueTask<bool> ToggleLikeAsync(long contentId)
    {
        var existLike = await unitOfWork.LikeRepository
            .SelectAsync(l => l.ContentId == contentId && l.UserId == HttpContextHelper.GetUserId);

        if (existLike is not null)
        {
            existLike.DeletedById = HttpContextHelper.GetUserId;
            await unitOfWork.LikeRepository.DeleteAsync(existLike);
            await unitOfWork.SaveAsync();
            return false;
        }

        var newLike = new Like
        {
            ContentId = contentId,
            UserId = HttpContextHelper.GetUserId,
            CreatedById = HttpContextHelper.GetUserId
        };

        await unitOfWork.LikeRepository.InsertAsync(newLike);
        await unitOfWork.SaveAsync();
        return true;
    }
} 