using SpeakingShorts.WebApi.Models.Stories;
using SpeakingShorts.WebApi.Models.UserCards;

namespace SpeakingShorts.WebApi.Models.MarkedWords
{
    public class MarkedWordViewModel
    {
        public long Id { get; set; }
        public string Word { get; set; }
        public string Definition { get; set; }
        public long StoryId { get; set; }
        public ICollection<UserCardViewModel> UserCards { get; set; }
    }
}
