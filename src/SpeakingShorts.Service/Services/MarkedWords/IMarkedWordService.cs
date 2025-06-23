using SpeakingShorts.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.MarkedWords;

public interface IMarkedWordService
{
    ValueTask<MarkedWord> CreateAsync(MarkedWord markedWord);
    ValueTask<MarkedWord> ModifyAsync(long id, MarkedWord markedWord);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<MarkedWord> GetAsync(long id);
    ValueTask<IEnumerable<MarkedWord>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<MarkedWord>> GetAllAsync();
    ValueTask<IEnumerable<MarkedWord>> GetByStoryIdAsync(long storyId);
} 