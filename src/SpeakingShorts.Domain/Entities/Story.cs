using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class Story : Auditable
{
    public string Title { get; set; }
    public ICollection<MarkedWord> MarkedWords { get; set; }
}
