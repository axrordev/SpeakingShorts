using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.WeeklyRankings;

public class WeeklyRankingService(IUnitOfWork unitOfWork) : IWeeklyRankingService
{

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existWeeklyRanking = await unitOfWork.WeeklyRankingRepository.SelectAsync(w => w.Id == id)
            ?? throw new NotFoundException("Weekly ranking not found");

        existWeeklyRanking.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.WeeklyRankingRepository.DeleteAsync(existWeeklyRanking);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<WeeklyRanking> GetAsync(long id)
    {
        return await unitOfWork.WeeklyRankingRepository
            .SelectAsync(expression: weeklyRanking => weeklyRanking.Id == id, includes: ["User"])
            ?? throw new NotFoundException("Weekly ranking not found");
    }

    public async ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var weeklyRankings = unitOfWork.WeeklyRankingRepository.Select(isTracking: false, includes: ["User"]);

        return await weeklyRankings.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync()
    {
        return await unitOfWork.WeeklyRankingRepository
            .Select(isTracking: false, includes: ["User"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<WeeklyRanking>> GetByUserIdAsync(long userId)
    {
        return await unitOfWork.WeeklyRankingRepository
            .Select(expression: weeklyRanking => weeklyRanking.UserId == userId)
            .ToListAsync();
    }


    private static int GetCurrentWeekNumber()
    {
        var today = DateTime.UtcNow;
        var firstDayOfYear = new DateTime(today.Year, 1, 1);
        var weekNumber = (today.DayOfYear - (int)firstDayOfYear.DayOfWeek + 6) / 7;
        return weekNumber;
    }

    private static DateTime GetWeekStartDate()
    {
        var today = DateTime.UtcNow;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        return today.AddDays(-1 * diff).Date;
    }

    private static DateTime GetWeekEndDate()
    {
        return GetWeekStartDate().AddDays(6);
    }

    public ValueTask<WeeklyRanking> CreateAsync(WeeklyRanking weeklyRanking)
    {
        throw new NotImplementedException();
    }

    public ValueTask<WeeklyRanking> ModifyAsync(long id, WeeklyRanking weeklyRanking)
    {
        throw new NotImplementedException();
    }

    public ValueTask<WeeklyRanking> GetCurrentWeekRankingAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<WeeklyRanking> GetLastWeekRankingAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<WeeklyRanking> GetByWeekNumberAsync(int weekNumber)
    {
        throw new NotImplementedException();
    }
} 