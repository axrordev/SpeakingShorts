using AutoMapper;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.Comments;
using SpeakingShorts.WebApi.Models.Comments;

namespace SpeakingShorts.WebApi.ApiService.Comments;

public class CommentApiService(ICommentService commentService, IMapper mapper) : ICommentApiService
{
    public async ValueTask<CommentViewModel> CreateAsync(CommentCreateModel model)
    {
        var comment = mapper.Map<Comment>(model);
        var createdComment = await commentService.CreateAsync(comment);
        return mapper.Map<CommentViewModel>(createdComment);
    }

    public async ValueTask<CommentViewModel> ModifyAsync(long id, CommentModifyModel model)
    {
        var comment = mapper.Map<Comment>(model);
        var modifiedComment = await commentService.ModifyAsync(id, comment);
        return mapper.Map<CommentViewModel>(modifiedComment);
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        return await commentService.DeleteAsync(id);
    }

    public async ValueTask<CommentViewModel> GetAsync(long id)
    {
        var comment = await commentService.GetAsync(id);
        return mapper.Map<CommentViewModel>(comment);
    }

    public async ValueTask<IEnumerable<CommentViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var comments = await commentService.GetAllAsync(@params, filter, search);
        return mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }

    public async ValueTask<IEnumerable<CommentViewModel>> GetAllAsync()
    {
        var comments = await commentService.GetAllAsync();
        return mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }

    public async ValueTask<IEnumerable<CommentViewModel>> GetByContentIdAsync(long contentId)
    {
        var comments = await commentService.GetByContentIdAsync(contentId);
        return mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }
}
