using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}

    public DbSet<Content> Contents { get; set; }
    public DbSet<BackgroundMusic> BackgroundMusics { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<WeeklyRanking> WeeklyRankings { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity relationships and properties
        modelBuilder.Entity<Content>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contents)
            .HasForeignKey(c => c.UserId);
        modelBuilder.Entity<Content>()
            .HasOne(c => c.BackgroundMusic)
            .WithMany()
            .HasForeignKey(c => c.BackgroundMusicId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Contents)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);


    }
}