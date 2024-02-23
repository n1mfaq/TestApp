using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace TestApp.ProgramStartUp;

public static class RegisterDependentServices
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        // Додати контролери та перегляди
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        // Додати конфігурацію DbContext
        AddDbContext(builder);

        // Додати SwaggerGen
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("V1", new OpenApiInfo { Title = "TestAppAPI", Version = "V1" });
        });

        // Встановлення налаштувань Npgsql для включення LegacyTimestampBehavior
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Налаштування логування
        builder.Logging.ClearProviders().AddConsole().AddDebug();

        return builder;
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging();
        });
    }
}