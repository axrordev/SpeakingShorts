using SpeakingShorts.Domain.Commons;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Domain.Entities;

public class WeeklyRanking : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long ContentId { get; set; }
    public Content Content { get; set; }
    public int LikeCount { get; set; }
    public int Rank { get; set; }
}