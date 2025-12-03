using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpeakingShorts.Data.DbContexts;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.WebApi.Seeding
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration config)
        {
            await context.Database.MigrateAsync();

            // 📌 Rollarni qo‘shish (agar mavjud bo‘lmasa)
            var roles = new[] { "user", "admin", "superadmin" };

            foreach (var roleName in roles)
            {
                if (!await context.UserRoles.AnyAsync(r => r.Name.ToLower() == roleName))
                {
                    context.UserRoles.Add(new UserRole
                    {
                        Name = roleName,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
            }

            await context.SaveChangesAsync();

            // 📌 SuperAdmin qo‘shish
            var superAdminLogin = config["SuperAdmin:Login"];
            var superAdminPassword = config["SuperAdmin:Password"];

            var superAdminRole = await context.UserRoles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == "superadmin");

            if (superAdminRole != null &&
                !await context.Users.AnyAsync(u => u.Email == superAdminLogin))
            {
                var passwordHash = PasswordHasher.Hash(superAdminPassword);

                context.Users.Add(new User
                {
                    Email = superAdminLogin,
                    PasswordHash = passwordHash,
                    UserRoleId = superAdminRole.Id,
                    RegisteredAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
