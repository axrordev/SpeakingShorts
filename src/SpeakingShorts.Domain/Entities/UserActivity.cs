using SpeakingShorts.Domain.Commons;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Domain.Entities;

public class UserActivity : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public DateTime UploadDate { get; set; }
    public bool IsContentDeleted { get; set; } 
}