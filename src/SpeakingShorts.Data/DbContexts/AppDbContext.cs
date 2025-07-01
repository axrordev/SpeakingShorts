using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}

    public DbSet<Content> Contents { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<WeeklyRanking> WeeklyRankings { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<MarkedWord> MarkedWords { get; set; }
    public DbSet<UserCard> UserCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserRole)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.UserRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Contents)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserActivities)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserCards)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Story>()
            .HasMany(s => s.MarkedWords)
            .WithOne(w => w.Story)
            .HasForeignKey(w => w.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // MarkedWord configuration
        modelBuilder.Entity<MarkedWord>()
            .HasOne(w => w.Story)
            .WithMany(s => s.MarkedWords)
            .HasForeignKey(w => w.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MarkedWord>()
            .HasMany(w => w.UserCards)
            .WithOne(c => c.MarkedWord)
            .HasForeignKey(c => c.MarkedWordId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserCard configuration
        modelBuilder.Entity<UserCard>()
            .HasOne(c => c.User)
            .WithMany(u => u.UserCards)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserCard>()
            .HasOne(c => c.MarkedWord)
            .WithMany(w => w.UserCards)
            .HasForeignKey(c => c.MarkedWordId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserRole configuration
        modelBuilder.Entity<UserRole>()
            .HasIndex(r => r.Name)
            .IsUnique();

        // UserRole seed data
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Name = "admin", CreatedAt = DateTime.UtcNow },
            new UserRole { Id = 2, Name = "user", CreatedAt = DateTime.UtcNow }
        );

        // Content configuration
        modelBuilder.Entity<Content>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contents)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Content>()
            .HasMany(c => c.Likes)
            .WithOne(l => l.Content)
            .HasForeignKey(l => l.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Content>()
            .HasMany(c => c.Comments)
            .WithOne(c => c.Content)
            .HasForeignKey(c => c.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Like configuration
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Content)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment configuration
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Content)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        // WeeklyRanking configuration
        modelBuilder.Entity<WeeklyRanking>()
            .HasOne(w => w.Content)
            .WithMany()
            .HasForeignKey(w => w.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserActivity configuration
        modelBuilder.Entity<UserActivity>()
            .HasOne(a => a.User)
            .WithMany(u => u.UserActivities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //Profile picture configuration
        modelBuilder.Entity<User>()
        .HasOne(u => u.ProfilePicture)
        .WithOne()
        .HasForeignKey<User>(u => u.ProfilePictureId)
        .OnDelete(DeleteBehavior.SetNull); // agar asset o'chirilsa, userda null bo'ladi

        // Global query filters
        modelBuilder.Entity<Content>().HasQueryFilter(c => c.IsDeleted == false);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsDeleted == false);
        modelBuilder.Entity<UserRole>().HasQueryFilter(ur => ur.IsDeleted == false);
        modelBuilder.Entity<Like>().HasQueryFilter(l => l.IsDeleted == false);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => c.IsDeleted == false);
        modelBuilder.Entity<WeeklyRanking>().HasQueryFilter(wr => wr.IsDeleted == false);
        modelBuilder.Entity<Announcement>().HasQueryFilter(a => a.IsDeleted == false);
        modelBuilder.Entity<Asset>().HasQueryFilter(a => a.IsDeleted == false);
        modelBuilder.Entity<UserActivity>().HasQueryFilter(ua => ua.IsDeleted == false);
        modelBuilder.Entity<Story>().HasQueryFilter(s => s.IsDeleted == false);
        modelBuilder.Entity<MarkedWord>().HasQueryFilter(w => w.IsDeleted == false);
        modelBuilder.Entity<UserCard>().HasQueryFilter(c => c.IsDeleted == false);
    }
}