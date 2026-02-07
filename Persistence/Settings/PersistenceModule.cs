using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Settings;

public static class PersistenceModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddScoped<INpgsqlDbContext>(sp =>
            new NpgsqlDbContext(
                // Optimized connection string for high concurrency:
                // - Increased Maximum Pool Size for stress testing with 100+ concurrent requests
                // - Increased Minimum Pool Size to reduce initial connection overhead
                // - Command Timeout allows queries more time before giving up
                // - Connection Idle Lifetime recycles idle connections to prevent stale connections
                // - No Connection Reuse is false to allow connection pooling/reuse
                "Host=localhost;Port=5432;Database=grocery;Username=postgres;Password=admin;Pooling=true;SSL Mode=Prefer;Trust Server Certificate=true;Minimum Pool Size=10;Maximum Pool Size=50;Connection Lifetime=300;Command Timeout=30;Connection Idle Lifetime=60;"));
    }
}