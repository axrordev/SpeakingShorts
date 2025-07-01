using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.MarkedWords;

namespace SpeakingShorts.WebApi.Models.Stories
{
    public class StoryViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<MarkedWordViewModel> MarkedWords { get; set; }
    }
}
