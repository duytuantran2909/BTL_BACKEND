using CommunityHallManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Khai báo dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Tranh loi serialize vong lap navigation properties (Hall -> Rooms -> Hall -> ...)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Reset DB + seed sample data (only when explicitly enabled)
using (var scope = app.Services.CreateScope())
{
    var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var reset = cfg.GetValue<bool>("Database:ResetOnStartup");
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (reset)
        await DbInitializer.ResetMigrateAndSeedAsync(db);
    else
        await DbInitializer.SeedIfEmptyAsync(db);
}

// QUAN TRỌNG: Kích hoạt CORS tại đây!
// Nó phải nằm trước app.MapControllers()
app.UseCors("AllowAll"); 

app.MapControllers();

app.Run();