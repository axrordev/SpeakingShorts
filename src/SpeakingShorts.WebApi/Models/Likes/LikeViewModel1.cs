using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.Models.Likes
{
    public class LikeViewModel
    {
        public long Id { get; set; }
        public UserViewModel User { get; set; } 
        public ContentViewModel Content { get; set; }
    }
}
