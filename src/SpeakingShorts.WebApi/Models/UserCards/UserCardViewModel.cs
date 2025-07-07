using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;
using SpeakingShorts.WebApi.Models.MarkedWords;

namespace SpeakingShorts.WebApi.Models.UserCards
{
    public class UserCardViewModel
    {
        public long Id { get; set; }
        public UserViewModel User { get; set; }
        public bool IsLearned { get; set; }
        public DateTime? LearnedAt { get; set; }
        public int ReviewCount { get; set; }
        public DateTime? LastReviewedAt { get; set; }
    }
}
