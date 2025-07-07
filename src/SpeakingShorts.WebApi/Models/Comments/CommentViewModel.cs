using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.Models.Comments;

public class CommentViewModel
{
    public long Id { get; set; }
    public UserViewModel User { get; set; }
    public long ContentId { get; set; }
    public string Text { get; set; }
}
