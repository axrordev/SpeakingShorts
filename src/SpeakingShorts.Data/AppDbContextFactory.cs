using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SpeakingShorts.Data.DbContexts;

namespace SpeakingShorts.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql("Server=localhost;Database=SpeakingShorts;Port=5432;User ID=postgres;Password=1001");

        return new AppDbContext(optionsBuilder.Options);
    }
}
