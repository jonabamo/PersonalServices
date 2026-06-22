using Microsoft.EntityFrameworkCore;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services;

public class IdempotencyService : IIdempotencyService
{
    private readonly AppDbContext _context;
    private readonly ILogger<IdempotencyService> _logger;
    private const int CacheDurationHours = 24;

    public IdempotencyService(AppDbContext context, ILogger<IdempotencyService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IdempotencyLog?> CheckAsync(string idempotencyKey, string requestHash)
    {
        try
        {
            var log = await _context.IdempotencyLogs
                .FirstOrDefaultAsync(l => l.IdempotencyKey == idempotencyKey);

            if (log == null)
            {
                _logger.LogDebug($"No idempotency log found for key: {idempotencyKey}");
                return null;
            }

            // Check if entry has expired
            if (log.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogInformation($"Idempotency log expired for key: {idempotencyKey}");
                await _context.IdempotencyLogs.Where(l => l.IdempotencyKey == idempotencyKey).ExecuteDeleteAsync();
                return null;
            }

            // Warn if request hash differs (different payload with same key)
            if (log.RequestHash != requestHash)
            {
                _logger.LogWarning($"Request hash mismatch for idempotency key {idempotencyKey}. Different payload detected.");
            }

            _logger.LogInformation($"Returning cached response for idempotency key: {idempotencyKey}");
            return log;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking idempotency log: {ex.Message}");
            return null;
        }
    }

    public async Task StoreAsync(string idempotencyKey, int statusCode, string responseBody, string requestHash, string httpMethod, string endpoint)
    {
        try
        {
            // Check if already exists (race condition protection)
            var existing = await _context.IdempotencyLogs
                .FirstOrDefaultAsync(l => l.IdempotencyKey == idempotencyKey);

            if (existing != null)
            {
                _logger.LogDebug($"Idempotency log already exists for key: {idempotencyKey}");
                return;
            }

            var log = new IdempotencyLog
            {
                IdempotencyKey = idempotencyKey,
                StatusCode = statusCode,
                ResponseBody = responseBody,
                RequestHash = requestHash,
                HttpMethod = httpMethod,
                Endpoint = endpoint,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(CacheDurationHours)
            };

            _context.IdempotencyLogs.Add(log);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Stored idempotency log for key: {idempotencyKey}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error storing idempotency log: {ex.Message}");
            throw;
        }
    }

    public async Task CleanupExpiredAsync()
    {
        try
        {
            var deletedCount = await _context.IdempotencyLogs
                .Where(l => l.ExpiresAt < DateTime.UtcNow)
                .ExecuteDeleteAsync();

            if (deletedCount > 0)
            {
                _logger.LogInformation($"Cleaned up {deletedCount} expired idempotency log entries");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during idempotency log cleanup: {ex.Message}");
        }
    }
}
