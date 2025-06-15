using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<BackgroundMusic> BackgroundMusics { get; set; }
    public DbSet<AppSettings> AppSettings { get; set; }

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

        // Content configuration
        modelBuilder.Entity<Content>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contents)
            .HasForeignKey(c => c.UserId)
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

        // AppSettings default values
        modelBuilder.Entity<AppSettings>().HasData(
            new AppSettings { Id = 1, Key = AppSettings.SettingKeys.MaxVideoLengthInSeconds, Value = "60", Description = "Maximum video length in seconds" },
            new AppSettings { Id = 2, Key = AppSettings.SettingKeys.MaxAudioLengthInSeconds, Value = "180", Description = "Maximum audio length in seconds" },
            new AppSettings { Id = 3, Key = AppSettings.SettingKeys.WeeklyTopCount, Value = "3", Description = "Number of top content to keep each week" },
            new AppSettings { Id = 4, Key = AppSettings.SettingKeys.AutoDeleteContentAfterDays, Value = "7", Description = "Number of days after which content is automatically deleted" }
        );
    }
} 