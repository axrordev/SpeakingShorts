using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.Comments;

public interface ICommentService
{
    ValueTask<Comment> CreateAsync(Comment comment);
    ValueTask<Comment> ModifyAsync(long id, Comment comment);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<Comment> GetAsync(long id);
    ValueTask<IEnumerable<Comment>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<Comment>> GetAllAsync();
    ValueTask<IEnumerable<Comment>> GetByContentIdAsync(long contentId);
} 