using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SpeakingShorts.Data.DbContexts;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.BackblazeServices;
using SpeakingShorts.Service.Services.Contents;
using SpeakingShorts.Service.Services.Processing;
using SpeakingShorts.WebApi.ApiService.Accounts;
using SpeakingShorts.WebApi.Extensions;
using SpeakingShorts.WebApi.MappingProfile;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger sozlash
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SpeakingShorts API",
        Version = "v1"
    });
});

// ✅ PostgreSQL uchun DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper konfiguratsiyasi
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ✅ Custom Services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddApiServices();


// Validatorlarni qo'shish
builder.Services.AddValidators();

// Swagger konfiguratsiyasi va xizmatlari
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

// JWT autentifikatsiyasini qo'shish
builder.Services.AddJwt(builder.Configuration);


// HTTP kontekst uchun qo'llanma
builder.Services.AddHttpContextAccessor();

// ✅ Backblaze konfiguratsiya va servis
builder.Services.Configure<BackblazeSettings>(builder.Configuration.GetSection("Backblaze"));
builder.Services.AddSingleton<IBackblazeService, BackblazeService>();

// Xatolikni boshqarish (Exception middleware'lari)
builder.Services.AddExceptions();

// ✅ Background Video Processing Services
builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx => 
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
    {
        queueCapacity = 100;
    }
    return new BackgroundTaskQueue(queueCapacity);
});
builder.Services.AddHostedService<VideoProcessingHostedService>();
builder.Services.AddScoped<IVideoProcessingService, VideoProcessingService>();

var app = builder.Build();

// CORS siyosatini ishlatish
app.UseCors("AllowSpecificOrigin");

// Qo'shimcha yordamchi funksiyalarni sozlash
app.AddInjectHelper();
app.AddPathInitializer();

// 🔹 Middleware
app.UseSwagger(); // Swagger har doim ochilsin
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpeakingShorts API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
