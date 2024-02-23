using Microsoft.EntityFrameworkCore;

namespace TestApp.Tests;

public class TestApplicationDbContextFactory : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private bool _disposed;

    public TestApplicationDbContextFactory()
    {
        // Используйте InMemoryDatabase или другие опции тестовой базы данных
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    public ApplicationDbContext CreateContext()
    {
        return new ApplicationDbContext(_options);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            // Удаляем тестовую базу данных после завершения тестов
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }

            _disposed = true;
        }
    }
}