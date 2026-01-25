using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Settings;

public static class PersistenceModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddScoped<INpgsqlDbContext>(sp =>
            new NpgsqlDbContext(
                "Host=localhost;Port=5432;Database=grocery;Username=postgres;Password=admin;Pooling=true;SSL Mode=Prefer;Trust Server Certificate=true;"));
    }
}