using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SpeakingShorts.Data.DbContexts;
using SpeakingShorts.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Servicelar
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpeakingShorts API", Version = "v1" });
});

// ✅ DbContext'ni build'dan oldin qo‘sh
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddServices();

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
