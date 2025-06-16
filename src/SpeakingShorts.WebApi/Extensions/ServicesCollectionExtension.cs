using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Service.Services.Accounts;
using SpeakingShorts.Service.Services.Assets;
using SpeakingShorts.Service.Services.UserRoles;
using SpeakingShorts.Service.Services.Users;

namespace SpeakingShorts.WebApi.Extensions;

public static class ServicesCollectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService,  AccountService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        //services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
       // services.AddScoped<ILikeService, LikeService>();
    }


}
