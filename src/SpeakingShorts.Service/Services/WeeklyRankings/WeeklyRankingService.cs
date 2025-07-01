using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.Infrastructure.Utilities;

namespace SpeakingShorts.Service.Services.WeeklyRankings
{
    public class WeeklyRankingService(IUnitOfWork unitOfWork, ISystemTime systemTime) : IWeeklyRankingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ISystemTime _systemTime = systemTime ?? throw new ArgumentNullException(nameof(systemTime));

        public async Task GenerateWeeklyRankingsAsync()
        {
            var weekStart = GetWeekStartDate();
            var weekEnd = GetWeekEndDate();

            // Content'larni olish va async qilish
            var activeContents = await _unitOfWork.ContentRepository.Select(c => !c.DeletedAt.HasValue, includes: ["Likes"]).ToListAsync();

            var userLikes = new Dictionary<long, int>();
            foreach (var content in activeContents)
            {
                var userId = content.UserId;
                if (!userLikes.ContainsKey(userId))
                    userLikes[userId] = 0;
                userLikes[userId] += content.Likes?.Count ?? 0;
            }

            var rankings = userLikes.Select(kv => new WeeklyRanking
            {
                UserId = kv.Key,
                LikeCount = kv.Value,
                CreatedById = 1 // Default yoki dinamik qiymat sifatida o'zgartirish mumkin
            }).ToList();

            // Mavjud reytinglarni tekshirish va faqat yangi qo'shish
            var existingUserIds = await _unitOfWork.WeeklyRankingRepository
                .Select(w => w.CreatedAt >= weekStart && w.CreatedAt <= weekEnd)
                .Select(w => w.UserId)
                .ToListAsync();

            var newRankings = rankings.Where(r => !existingUserIds.Contains(r.UserId)).ToList();

            // InsertRangeAsync o'rniga InsertAsync ni tsikl bilan ishlatamiz
            foreach (var ranking in newRankings)
            {
                await _unitOfWork.WeeklyRankingRepository.InsertAsync(ranking);
            }
            await _unitOfWork.SaveAsync();

            await UpdateRanksAsync();
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            var existWeeklyRanking = await _unitOfWork.WeeklyRankingRepository.SelectAsync(w => w.Id == id)
                ?? throw new NotFoundException("Weekly ranking not found");
            existWeeklyRanking.DeletedById = HttpContextHelper.GetUserId;
            await _unitOfWork.WeeklyRankingRepository.DeleteAsync(existWeeklyRanking);
            await _unitOfWork.SaveAsync();
            await UpdateRanksAsync();
            return true;
        }

        public async ValueTask<WeeklyRanking> GetAsync(long id)
        {
            return await _unitOfWork.WeeklyRankingRepository
                .SelectAsync(w => w.Id == id, includes: ["User", "Content"])
                ?? throw new NotFoundException("Weekly ranking not found");
        }

        public async ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var query = _unitOfWork.WeeklyRankingRepository.Select(isTracking: false, includes: ["User", "Content"]);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(w => w.User.Username.ToLower().Contains(search.ToLower()) || w.Content.Title.ToLower().Contains(search.ToLower()));
            return await query.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
        }

        public async ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync()
        {
            return await _unitOfWork.WeeklyRankingRepository.Select(isTracking: false, includes: ["User", "Content"]).ToListAsync();
        }

        public async ValueTask<IEnumerable<WeeklyRanking>> GetByUserIdAsync(long userId)
        {
            return await _unitOfWork.WeeklyRankingRepository.Select(w => w.UserId == userId, includes: ["Content"]).ToListAsync();
        }

        public async ValueTask<WeeklyRanking> GetCurrentWeekRankingAsync()
        {
            var weekStart = GetWeekStartDate();
            var weekEnd = GetWeekEndDate();
            return await _unitOfWork.WeeklyRankingRepository
                .SelectAsync(w => w.CreatedAt >= weekStart && w.CreatedAt <= weekEnd && w.UserId == HttpContextHelper.GetUserId, includes: ["User", "Content"])
                ?? throw new NotFoundException("No ranking found for the current week");
        }

        public async ValueTask<WeeklyRanking> GetLastWeekRankingAsync()
        {
            var weekStart = GetWeekStartDate().AddDays(-7);
            var weekEnd = GetWeekEndDate().AddDays(-7);
            return await _unitOfWork.WeeklyRankingRepository
                .SelectAsync(w => w.CreatedAt >= weekStart && w.CreatedAt <= weekEnd && w.UserId == HttpContextHelper.GetUserId, includes: ["User", "Content"])
                ?? throw new NotFoundException("No ranking found for the last week");
        }

        public async ValueTask<WeeklyRanking> GetByWeekNumberAsync(int weekNumber)
        {
            var year = _systemTime.UtcNow.Year;
            var firstDayOfYear = new DateTime(year, 1, 1);
            var weekStart = firstDayOfYear.AddDays((weekNumber - 1) * 7 - (int)firstDayOfYear.DayOfWeek + 1);
            var weekEnd = weekStart.AddDays(6);
            return await _unitOfWork.WeeklyRankingRepository
                .SelectAsync(w => w.CreatedAt >= weekStart && w.CreatedAt <= weekEnd && w.UserId == HttpContextHelper.GetUserId, includes: ["User", "Content"])
                ?? throw new NotFoundException($"No ranking found for week {weekNumber}");
        }

        private async Task UpdateRanksAsync()
        {
            var rankings = await _unitOfWork.WeeklyRankingRepository.Select().OrderByDescending(w => w.LikeCount).ToListAsync();
            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i].Rank = i + 1;
                await _unitOfWork.WeeklyRankingRepository.UpdateAsync(rankings[i]);
            }
            await _unitOfWork.SaveAsync();
        }

        private DateTime GetWeekStartDate()
        {
            var today = _systemTime.UtcNow;
            var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            return today.AddDays(-1 * diff).Date;
        }

        private DateTime GetWeekEndDate()
        {
            return GetWeekStartDate().AddDays(6);
        }
    }
}   