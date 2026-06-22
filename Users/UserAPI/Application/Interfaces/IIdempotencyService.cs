using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces;

/// <summary>
/// Service for handling idempotent request tracking and response caching
/// </summary>
public interface IIdempotencyService
{
    /// <summary>
    /// Check if an idempotency key exists in the log and return cached response if found
    /// </summary>
    /// <param name="idempotencyKey">Unique key for the request</param>
    /// <param name="requestHash">SHA256 hash of the request body</param>
    /// <returns>Cached IdempotencyLog if found, null otherwise</returns>
    Task<IdempotencyLog?> CheckAsync(string idempotencyKey, string requestHash);

    /// <summary>
    /// Store the response of an operation in the idempotency log
    /// </summary>
    /// <param name="idempotencyKey">Unique key for the request</param>
    /// <param name="statusCode">HTTP status code of the response</param>
    /// <param name="responseBody">Serialized response body</param>
    /// <param name="requestHash">SHA256 hash of the request body</param>
    /// <param name="httpMethod">HTTP method (POST, PUT, DELETE, etc.)</param>
    /// <param name="endpoint">API endpoint path</param>
    Task StoreAsync(string idempotencyKey, int statusCode, string responseBody, string requestHash, string httpMethod, string endpoint);

    /// <summary>
    /// Clean up expired idempotency logs (older than 24 hours)
    /// </summary>
    Task CleanupExpiredAsync();
}
