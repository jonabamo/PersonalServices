namespace UserAPI.Domain.Entities;

public class IdempotencyLog
{
    /// <summary>
    /// Unique idempotency key provided by the client
    /// </summary>
    public string IdempotencyKey { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code of the original response (e.g., 201, 200, 400)
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Serialized response body for replaying to retries
    /// </summary>
    public string ResponseBody { get; set; } = string.Empty;

    /// <summary>
    /// SHA256 hash of the request body to detect if retry has modified payload
    /// </summary>
    public string RequestHash { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when this entry was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when this entry expires and should be cleaned up (24 hours after creation)
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// HTTP method (POST, PUT, DELETE, etc.)
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>
    /// API endpoint path for tracking purposes
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
}
