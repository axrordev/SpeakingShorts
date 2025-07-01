using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using SpeakingShorts.Data.Repositories;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Domain.Entities;

namespace SpeakingShorts.Data.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> UserRepository { get; }
    IRepository<UserRole> UserRoleRepository { get; }
    IRepository<Announcement> AnnouncementRepository { get; }
    IRepository<Comment> CommentRepository { get; }
    IRepository<Asset> AssetRepository { get; }
    IRepository<Content> ContentRepository { get; }
    IRepository<Like> LikeRepository { get; }
    IRepository<MarkedWord> MarkedWordRepository { get; }
    IRepository<Story> StoryRepository { get; }
    IRepository<UserActivity> UserActivityRepository { get; }
    IRepository<UserCard> UserCardRepository { get; }
    IRepository<WeeklyRanking> WeeklyRankingRepository { get; }

    ValueTask<bool> SaveAsync();
    ValueTask BeginTransactionAsync();
    ValueTask CommitTransactionAsync();
    ValueTask Rollback();
}
