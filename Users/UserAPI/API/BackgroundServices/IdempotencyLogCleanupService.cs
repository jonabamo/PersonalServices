using UserAPI.Application.Interfaces;

namespace UserAPI.API.BackgroundServices;

/// <summary>
/// Background service that periodically cleans up expired idempotency logs
/// </summary>
public class IdempotencyLogCleanupService : BackgroundService
{
    private readonly ILogger<IdempotencyLogCleanupService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const int CleanupIntervalMinutes = 60; // Run cleanup every hour

    public IdempotencyLogCleanupService(ILogger<IdempotencyLogCleanupService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("IdempotencyLogCleanupService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var idempotencyService = scope.ServiceProvider.GetRequiredService<IIdempotencyService>();
                    await idempotencyService.CleanupExpiredAsync();
                }

                // Wait for the specified interval before next cleanup
                await Task.Delay(TimeSpan.FromMinutes(CleanupIntervalMinutes), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("IdempotencyLogCleanupService is shutting down");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in IdempotencyLogCleanupService: {ex.Message}");
                // Continue running even if an error occurs
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("IdempotencyLogCleanupService stopped");
    }
}
