using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.Models.Likes
{
    public class LikeViewModel
    {
        public long Id { get; set; }
        public UserViewModel User { get; set; } 
        public long ContentId { get; set; }
    }
}
