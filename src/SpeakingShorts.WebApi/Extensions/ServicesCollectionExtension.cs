using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.Accounts;
using SpeakingShorts.Service.Services.Assets;
using SpeakingShorts.Service.Services.BackgroundMusics;
using SpeakingShorts.Service.Services.Contents;
using SpeakingShorts.Service.Services.Processing;
using SpeakingShorts.Service.Services.UserRoles;
using SpeakingShorts.Service.Services.Users;
using SpeakingShorts.WebApi.ApiService.Accounts;
using SpeakingShorts.WebApi.ApiService.Users;
using SpeakingShorts.WebApi.Helpers;
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
        services.AddScoped<IBackgroundMusicService, BackgroundMusicService>();
        //services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
       // services.AddScoped<ILikeService, LikeService>();

        // Content and Video Processing
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<IVideoProcessingService, VideoProcessingService>();

        // Background Task Queue
        services.AddSingleton<IBackgroundTaskQueue>(ctx => 
        {
            if (!int.TryParse(configuration["QueueCapacity"], out var queueCapacity))
            {
                queueCapacity = 100;
            }
            return new BackgroundTaskQueue(queueCapacity);
        });
        services.AddHostedService<VideoProcessingHostedService>();
    }

     public static void AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountApiService, AccountApiService>();
            services.AddScoped<IUserApiService, UserApiService>();

        }

        public static void AddExceptions(this IServiceCollection services)
        {
            FilePathHelper.WwwrootPath = Path.GetFullPath("wwwroot");

            //services.AddExceptionHandler<NotFoundExceptionMiddleware>();
            //services.AddExceptionHandler<ForbiddenExceptionMiddleware>();
            //services.AddExceptionHandler<AlreadyExistExceptionMiddleware>();
            //services.AddExceptionHandler<InternalServerExceptionMiddleware>();
            //services.AddExceptionHandler<ArgumentIsNotValidExceptionMiddleware>();
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
        services.AddSwaggerGen(setup =>
        {
            // JWT autentifikatsiyasi uchun Swagger konfiguratsiyasi
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });        
        });
    }
}
