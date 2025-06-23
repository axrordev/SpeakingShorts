using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.Comments;

public class CommentService(IUnitOfWork unitOfWork) : ICommentService
{
    public async ValueTask<Comment> CreateAsync(Comment comment)
    {
        comment.UserId = HttpContextHelper.GetUserId;
        comment.CreatedById = HttpContextHelper.GetUserId;

        var createdComment = await unitOfWork.CommentRepository.InsertAsync(comment);
        await unitOfWork.SaveAsync();
        return createdComment;
    }

    public async ValueTask<Comment> ModifyAsync(long id, Comment comment)
    {
        var existComment = await unitOfWork.CommentRepository.SelectAsync(c => c.Id == id)
            ?? throw new NotFoundException("Comment not found");

        if (existComment.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to modify this comment");

        existComment.Text = comment.Text;
        existComment.UpdatedById = HttpContextHelper.GetUserId;
        existComment.UpdatedAt = DateTime.UtcNow;

        var updatedComment = await unitOfWork.CommentRepository.UpdateAsync(existComment);
        await unitOfWork.SaveAsync();

        return updatedComment;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existComment = await unitOfWork.CommentRepository.SelectAsync(c => c.Id == id)
            ?? throw new NotFoundException("Comment not found");

        if (existComment.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to delete this comment");

        existComment.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.CommentRepository.DeleteAsync(existComment);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<Comment> GetAsync(long id)
    {
        return await unitOfWork.CommentRepository
            .SelectAsync(expression: comment => comment.Id == id, includes: ["User", "Content"])
            ?? throw new NotFoundException("Comment not found");
    }

    public async ValueTask<IEnumerable<Comment>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var comments = unitOfWork.CommentRepository.Select(isTracking: false, includes: ["User", "Content"]);

        if (!string.IsNullOrWhiteSpace(search))
            comments = comments.Where(c => c.Text.ToLower().Contains(search.ToLower()));

        return await comments.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<IEnumerable<Comment>> GetAllAsync()
    {
        return await unitOfWork.CommentRepository
            .Select(isTracking: false, includes: ["User", "Content"])
            .ToListAsync();
    }

    public async ValueTask<IEnumerable<Comment>> GetByContentIdAsync(long contentId)
    {
        return await unitOfWork.CommentRepository
            .Select(expression: comment => comment.ContentId == contentId, includes: ["User"])
            .ToListAsync();
    }
} 