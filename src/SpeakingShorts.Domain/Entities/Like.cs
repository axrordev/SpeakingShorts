using System;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class Like : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long ContentId { get; set; }
    public Content Content { get; set; }
} 