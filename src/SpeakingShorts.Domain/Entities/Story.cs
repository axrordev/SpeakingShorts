using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class Story : Auditable
{
    public string Title { get; set; }
    public long AdminId { get; set; }
    public User Admin { get; set; }
    public ICollection<MarkedWord> MarkedWords { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
}

public class MarkedWord : Auditable
{
    public string Word { get; set; }
    public string Definition { get; set; }
    public long StoryId { get; set; }
    public Story Story { get; set; }
    public ICollection<UserCard> UserCards { get; set; }
}

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