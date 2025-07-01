using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.UserCards
{
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

        public async ValueTask<UserCard> ModifyAsync(long id, UserCard userCard)
        {
            var existUserCard = await unitOfWork.UserCardRepository.SelectAsync(u => u.Id == id)
                ?? throw new NotFoundException("User card not found");

            if (existUserCard.UserId != HttpContextHelper.GetUserId)
                throw new ForbiddenException("You are not allowed to modify this user card");

            existUserCard.IsLearned = userCard.IsLearned;
            existUserCard.LearnedAt = userCard.LearnedAt;
            existUserCard.ReviewCount = userCard.ReviewCount;
            existUserCard.LastReviewedAt = userCard.LastReviewedAt;
            existUserCard.UpdatedById = HttpContextHelper.GetUserId;
            existUserCard.UpdatedAt = DateTime.UtcNow;

            var updatedUserCard = await unitOfWork.UserCardRepository.UpdateAsync(existUserCard);
            await unitOfWork.SaveAsync();

            return updatedUserCard;
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
                .SelectAsync(expression: userCard => userCard.Id == id, includes: ["User", "MarkedWord"])
                ?? throw new NotFoundException("User card not found");
        }

        public async ValueTask<IEnumerable<UserCard>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var userCards = unitOfWork.UserCardRepository.Select(isTracking: false, includes: ["User", "MarkedWord"]);

            return await userCards.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
        }

        public async ValueTask<IEnumerable<UserCard>> GetAllAsync()
        {
            return await unitOfWork.UserCardRepository
                .Select(isTracking: false, includes: ["User", "MarkedWord"])
                .ToListAsync();
        }

        public async ValueTask<IEnumerable<UserCard>> GetByUserIdAsync(long userId)
        {
            return await unitOfWork.UserCardRepository
                .Select(expression: userCard => userCard.UserId == userId, includes: ["MarkedWord"])
                .ToListAsync();
        }

        public async ValueTask<UserCard> GetByCardIdAsync(long cardId)
        {
            return await unitOfWork.UserCardRepository
                .SelectAsync(expression: userCard => userCard.MarkedWordId == cardId && userCard.UserId == HttpContextHelper.GetUserId, includes: ["MarkedWord"])
                ?? throw new NotFoundException("User card not found for the specified card ID");
        }
    }
}