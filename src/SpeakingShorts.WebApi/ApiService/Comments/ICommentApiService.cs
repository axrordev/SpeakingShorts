using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Comments;

namespace SpeakingShorts.WebApi.ApiService.Comments;

public interface ICommentApiService
{
    ValueTask<CommentViewModel> CreateAsync(CommentCreateModel model);
    ValueTask<CommentViewModel> ModifyAsync(long id, CommentModifyModel model);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CommentViewModel> GetAsync(long id);
    ValueTask<IEnumerable<CommentViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CommentViewModel>> GetAllAsync();
    ValueTask<IEnumerable<CommentViewModel>> GetByContentIdAsync(long contentId);
}
