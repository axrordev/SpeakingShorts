using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities.Users;

public class UserRole : Auditable
{
    public string Name { get; set; }
    public ICollection<User> Users { get; set; } 
}