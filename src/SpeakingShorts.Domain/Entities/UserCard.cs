using SpeakingShorts.Domain.Commons;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Domain.Entities;

public class UserCard : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long MarkedWordId { get; set; }
    public MarkedWord MarkedWord { get; set; }
    public bool IsLearned { get; set; }
    public DateTime? LearnedAt { get; set; }
    public int ReviewCount { get; set; }
    public DateTime? LastReviewedAt { get; set; }
} 