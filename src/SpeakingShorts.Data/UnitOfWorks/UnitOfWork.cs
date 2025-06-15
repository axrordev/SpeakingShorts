
using SpeakingShorts.Data.DbContexts;
using SpeakingShorts.Data.Repositories;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Data.UnitOfWorks;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext context = context;
    public IRepository<Comment> CommentRepository { get; } = new Repository<Comment>(context);
    public IRepository<User> UserRepository { get; } = new Repository<User>(context);
    public IRepository<UserRole> UserRoleRepository { get; } = new Repository<UserRole>(context);
    public IRepository<Announcement> AnnouncementRepository { get; } = new Repository<Announcement>(context);
    public IRepository<Asset> AssetRepository { get; } = new Repository<Asset>(context);
    public IRepository<BackgroundMusic> BackgroundMusicRepository { get; } = new Repository<BackgroundMusic>(context);
    public IRepository<Content> ContentRepository { get; } = new Repository<Content>(context);
    public IRepository<Like> LikeRepository { get; } = new Repository<Like>(context);
    public IRepository<MarkedWord> MarkedWordRepository { get; } = new Repository<MarkedWord>(context);
    public IRepository<Story> StoryRepository { get; } = new Repository<Story>(context);
    public IRepository<UserActivity> UserActivityRepository { get; } = new Repository<UserActivity>(context);
    public IRepository<UserCard> UserCardRepository { get; } = new Repository<UserCard>(context);
    public IRepository<WeeklyRanking> WeeklyRankingRepository { get; } = new Repository<WeeklyRanking>(context);


    public async ValueTask BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public async ValueTask CommitTransactionAsync()
    {
        await context.Database.CommitTransactionAsync();
    }

    public async ValueTask Rollback()
    {
        await context.Database.RollbackTransactionAsync();
    }

    public async ValueTask<bool> SaveAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}