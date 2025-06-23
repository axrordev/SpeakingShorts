using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.MarkedWords;

public class MarkedWordService(IUnitOfWork unitOfWork) : IMarkedWordService
{
    public async ValueTask<MarkedWord> CreateAsync(MarkedWord markedWord)
    {
        var existMarkedWord = await unitOfWork.MarkedWordRepository
            .SelectAsync(m => m.StoryId == markedWord.StoryId && m.Word.ToLower() == markedWord.Word.ToLower()) ;

        if (existMarkedWord is not null)
            throw new AlreadyExistException($"This word is already marked in this story | Word={markedWord.Word}");

        var newMarkedWord = new MarkedWord
        {
            StoryId = markedWord.StoryId,
            Word = markedWord.Word,
            Definition = markedWord.Definition,
            CreatedById = HttpContextHelper.GetUserId
        };

        var createdMarkedWord = await unitOfWork.MarkedWordRepository.InsertAsync(newMarkedWord);
        await unitOfWork.SaveAsync();

        return createdMarkedWord;
    }

    public async ValueTask<MarkedWord> ModifyAsync(long id, MarkedWord markedWord)
    {
        var existMarkedWord = await unitOfWork.MarkedWordRepository.SelectAsync(m => m.Id == id)
            ?? throw new NotFoundException("Marked word not found");

        existMarkedWord.Word = markedWord.Word;
        existMarkedWord.Definition= markedWord.Definition;
        existMarkedWord.UpdatedById = HttpContextHelper.GetUserId;
        existMarkedWord.UpdatedAt = DateTime.UtcNow;

        var updatedMarkedWord = await unitOfWork.MarkedWordRepository.UpdateAsync(existMarkedWord);
        await unitOfWork.SaveAsync();

        return updatedMarkedWord;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existMarkedWord = await unitOfWork.MarkedWordRepository.SelectAsync(m => m.Id == id)
            ?? throw new NotFoundException("Marked word not found");

        existMarkedWord.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.MarkedWordRepository.DeleteAsync(existMarkedWord);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<MarkedWord> GetAsync(long id)
    {
        return await unitOfWork.MarkedWordRepository
            .SelectAsync(expression: markedWord => markedWord.Id == id, includes: ["Story"])
            ?? throw new NotFoundException("Marked word not found");
    }

    public async ValueTask<IEnumerable<MarkedWord>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var markedWords = unitOfWork.MarkedWordRepository.Select(isTracking: false, includes: ["Content"]);

        if (!string.IsNullOrWhiteSpace(search))
            markedWords = markedWords.Where(m => 
                m.Word.ToLower().Contains(search.ToLower()));

        return await markedWords.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<MarkedWord>> GetAllAsync()
    {
        return await unitOfWork.MarkedWordRepository
            .Select(isTracking: false, includes: ["Content"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<MarkedWord>> GetByStoryIdAsync(long contentId)
    {
        return await unitOfWork.MarkedWordRepository
            .Select(expression: markedWord => markedWord.StoryId == contentId)
            .ToListAsync();
    }
} 