using SpeakingShorts.Domain.Entities;

namespace SpeakingShorts.WebApi.Models.MarkedWords
{
    public class MarkedWordCreateModel
    {
        public string Word { get; set; }
        public string Definition { get; set; }
        public long StoryId { get; set; }
    }
}
