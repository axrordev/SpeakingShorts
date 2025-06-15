using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class Asset : Auditable
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}
