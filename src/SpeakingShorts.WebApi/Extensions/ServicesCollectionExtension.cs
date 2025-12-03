
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.Accounts;
using SpeakingShorts.Service.Services.Announcements;
using SpeakingShorts.Service.Services.Assets;
using SpeakingShorts.Service.Services.Comments;
using SpeakingShorts.Service.Services.Contents;
using SpeakingShorts.Service.Services.Likes;
using SpeakingShorts.Service.Services.MarkedWords;
using SpeakingShorts.Service.Services.Stories;
using SpeakingShorts.Service.Services.UserActivities;
using SpeakingShorts.Service.Services.UserCards;
using SpeakingShorts.Service.Services.UserRoles;
using SpeakingShorts.Service.Services.Users;
using SpeakingShorts.Service.Services.WeeklyRankings;
using SpeakingShorts.WebApi.ApiService.Accounts;
using SpeakingShorts.WebApi.ApiService.Announcements;
using SpeakingShorts.WebApi.ApiService.Comments;
using SpeakingShorts.WebApi.ApiService.Contents;
using SpeakingShorts.WebApi.ApiService.Likes;
using SpeakingShorts.WebApi.ApiService.MarkedWords;
using SpeakingShorts.WebApi.ApiService.Stories;
using SpeakingShorts.WebApi.ApiService.UserCards;
using SpeakingShorts.WebApi.ApiService.UserRoles;
using SpeakingShorts.WebApi.ApiService.Users;
using SpeakingShorts.WebApi.ApiService.WeeklyRankings;
using SpeakingShorts.WebApi.Helpers;
using SpeakingShorts.WebApi.Middlewares;
using SpeakingShorts.WebApi.Validators.Accounts;
using System.Text;

namespace SpeakingShorts.WebApi.Extensions;

public static class ServicesCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Memory Cache
        services.AddMemoryCache();
       
        // Register services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<IMarkedWordService, MarkedWordService>();
        services.AddScoped<IStoryService, StoryService>();
        services.AddScoped<IUserCardService, UserCardService>();
        services.AddScoped<IWeeklyRankingService, WeeklyRankingService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<IUserActivityService, UserActivityService>();
    }

     public static void AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountApiService, AccountApiService>();
            services.AddScoped<IUserApiService, UserApiService>();
            services.AddScoped<IAnnouncementApiService, AnnouncementApiService>();
            services.AddScoped<ICommentApiService, CommentApiService>();
            services.AddScoped<ILikeApiService, LikeApiService>();
            services.AddScoped<IMarkedWordApiService, MarkedWordApiService>();
            services.AddScoped<IStoryApiService, StoryApiService>();
            services.AddScoped<IUserCardApiService, UserCardApiService>();
            services.AddScoped<IWeeklyRankingApiService, WeeklyRankingApiService>();
            services.AddScoped<IContentApiService, ContentApiService>();
            services.AddScoped<IUserRoleApiService, UserRoleApiService>();
        }

        public static void AddExceptions(this IServiceCollection services)
        {
            FilePathHelper.WwwrootPath = Path.GetFullPath("wwwroot");

        services.AddExceptionHandler<NotFoundExceptionMiddleware>();
        services.AddExceptionHandler<ForbiddenExceptionMiddleware>();
        services.AddExceptionHandler<AlreadyExistExceptionMiddleware>();
        services.AddExceptionHandler<InternalServerExceptionMiddleware>();
        services.AddExceptionHandler<ArgumentIsNotValidExceptionMiddleware>();
    }

        public static void AddInjectHelper(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            ServiceHelper.UserRoleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();
        }

        public static void AddPathInitializer(this WebApplication app)
    {
        HttpContextHelper.ContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        EnvironmentHelper.JwtKey = app.Configuration.GetSection("Jwt:Key").Value;
        EnvironmentHelper.TokenLifeTimeInHour = app.Configuration.GetSection("Jwt:LifeTime").Value;
        EnvironmentHelper.SmtpHost = app.Configuration.GetSection("Email:SmtpHost").Value;
        EnvironmentHelper.SmtpPort = app.Configuration.GetSection("Email:SmtpPort").Value;
        EnvironmentHelper.EmailAddress = app.Configuration.GetSection("Email:EmailAddress").Value;
        EnvironmentHelper.EmailPassword = app.Configuration.GetSection("Email:EmailPassword").Value;
        EnvironmentHelper.SuperAdminLogin = app.Configuration.GetSection("SuperAdmin:Login").Value;
        EnvironmentHelper.SuperAdminPassword = app.Configuration.GetSection("SuperAdmin:Password").Value;
    }

        public static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<AccountLoginValidator>();
        services.AddTransient<AccountCreateValidator>();
        services.AddTransient<AccountVerifyValidator>();
        services.AddTransient<AccountSendCodeValidator>();
        services.AddTransient<AccountRegisterModelValidator>();
        services.AddTransient<AccountResetPasswordValidator>();
    }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }


     public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SpeakingShorts API",
                Version = "v1",
                Description = "API for SpeakingShorts application"
            });

            // JWT Bearer auth configuration
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer token **_without Bearer prefix_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            // Add security definition
            c.AddSecurityDefinition("Bearer", securityScheme);

            // Add security requirement
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    Array.Empty<string>()
                }
            });
        });
    }


}
