using TestApp.Services;

namespace TestApp.ProgramStartUp;

public static class DependencyInjections
{
    public static WebApplicationBuilder Inject(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOptionChooserService, OptionChooserService>();
        return builder;
    }
}