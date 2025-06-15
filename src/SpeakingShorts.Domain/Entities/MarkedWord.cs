using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class MarkedWord : Auditable
{
    public string Word { get; set; }
    public string Definition { get; set; }
    public long StoryId { get; set; }
    public Story Story { get; set; }
    public ICollection<UserCard> UserCards { get; set; }
}
