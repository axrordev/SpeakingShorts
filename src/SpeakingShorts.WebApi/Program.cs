using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.DbContexts;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Services.BackblazeServices;
using SpeakingShorts.Service.Services.Infrastructure.Utilities;
using SpeakingShorts.Service.Services.WeeklyRankings;
using SpeakingShorts.WebApi.Extensions;
using SpeakingShorts.WebApi.MappingProfile;
using SpeakingShorts.WebApi.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");


// 🔹 Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ PostgreSQL uchun DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper konfiguratsiyasi
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ✅ Custom Services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddApiServices();

// Hosted Service
builder.Services.AddHostedService<WeeklyRankingJob>();

// Singleton xizmatlar
builder.Services.AddSingleton<ISystemTime, SystemTime>();

// Validatorlarni qo'shish
builder.Services.AddValidators();

// ✅ Swagger konfiguratsiyasi (faqat ConfigureSwagger)
builder.Services.ConfigureSwagger(); 

// JWT autentifikatsiyasini qo'shish
builder.Services.AddJwt(builder.Configuration);

// HTTP kontekst uchun qo'llanma
builder.Services.AddHttpContextAccessor();
// CORS siyosatini qo'shish (frontend URL'ini qo'shing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://speakingshorts.uz")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// ✅ Backblaze konfiguratsiya va servis
builder.Services.Configure<BackblazeSettings>(builder.Configuration.GetSection("Backblaze"));
builder.Services.AddSingleton<IBackblazeService, BackblazeService>();

// Xatolikni boshqarish (Exception middleware'lari)
builder.Services.AddExceptions();

var app = builder.Build();

// Ma’lumotlar bazasini init qilish
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(dbContext, builder.Configuration);
}

// CORS siyosatini ishlatish
app.UseCors("AllowSpecificOrigin");

// Qo'shimcha yordamchi funksiyalarni sozlash
app.AddInjectHelper();
app.AddPathInitializer();

// 🔹 Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpeakingShorts API V1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
