using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.UserCards;

public class UserCardService(IUnitOfWork unitOfWork) : IUserCardService
{
    public async ValueTask<UserCard> CreateAsync(UserCard userCard)
    {
        userCard.UserId = HttpContextHelper.GetUserId;
        userCard.CreatedById = HttpContextHelper.GetUserId;

        var createdUserCard = await unitOfWork.UserCardRepository.InsertAsync(userCard);
        await unitOfWork.SaveAsync();
        return createdUserCard;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existUserCard = await unitOfWork.UserCardRepository.SelectAsync(u => u.Id == id)
            ?? throw new NotFoundException("User card not found");

        if (existUserCard.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to delete this user card");

        existUserCard.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.UserCardRepository.DeleteAsync(existUserCard);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<UserCard> GetAsync(long id)
    {
        return await unitOfWork.UserCardRepository
            .SelectAsync(expression: userCard => userCard.Id == id, includes: ["User", "Card"])
            ?? throw new NotFoundException("User card not found");
    }

    public async ValueTask<IEnumerable<UserCard>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var userCards = unitOfWork.UserCardRepository.Select(isTracking: false, includes: ["User", "Card"]);

        return await userCards.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<UserCard>> GetAllAsync()
    {
        return await unitOfWork.UserCardRepository
            .Select(isTracking: false, includes: ["User", "Card"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<UserCard>> GetByUserIdAsync(long userId)
    {
        return await unitOfWork.UserCardRepository
            .Select(expression: userCard => userCard.UserId == userId, includes: ["Card"])
            .ToListAsync();
    }

    public ValueTask<UserCard> ModifyAsync(long id, UserCard userCard)
    {
        throw new NotImplementedException();
    }

    public ValueTask<UserCard> GetByCardIdAsync(long cardId)
    {
        throw new NotImplementedException();
    }
} 