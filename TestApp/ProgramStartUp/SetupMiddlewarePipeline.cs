namespace TestApp.ProgramStartUp;

public static class SetupMiddlewarePipeline
{
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            // UseExceptionHandler для обробки винятків
            app.UseExceptionHandler("/Home/Error");

            // Налаштування HSTS (HTTP Strict Transport Security)
            app.UseHsts();
        }

        // Видалити UseDeveloperExceptionPage для продуктового середовища
        // app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/V1/swagger.json", "TestAppAPI V1"); });

        // Використання MapDefaultControllerRoute для налаштування маршрутів за замовчуванням
        app.MapDefaultControllerRoute();

        return app;
    }
}