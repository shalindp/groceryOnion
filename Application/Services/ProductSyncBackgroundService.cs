using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public sealed class ProductSyncBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProductSyncBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public ProductSyncBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ProductSyncBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Product sync service started");

        // 🔥 Run immediately on startup
        await RunOnceSafely(stoppingToken);

        // ⏱️ Then every hour
        using var timer = new PeriodicTimer(_interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunOnceSafely(stoppingToken);
        }
    }

    private async Task RunOnceSafely(CancellationToken token)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var job = scope.ServiceProvider.GetRequiredService<ProductSyncJob>();

            await job.RunAsync(token);
        }
        catch (OperationCanceledException)
        {
            // App shutting down — expected
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Product sync failed");
        }
    }
}