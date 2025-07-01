using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.Models.WeeklyRankings
{
    public class WeeklyRankingViewModel
    {
        public long Id { get; set; }
        public UserViewModel User { get; set; }
        public ContentViewModel Content { get; set; }
        public int LikeCount { get; set; }
        public int Rank { get; set; }
    }
}
