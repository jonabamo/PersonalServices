using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using UserAPI.Application.Interfaces;

namespace UserAPI.API.Middleware;

/// <summary>
/// Middleware for handling idempotent requests.
/// Checks for Idempotency-Key header on POST, PUT, DELETE operations.
/// Returns cached response if retry is detected, otherwise stores response for future retries.
/// </summary>
public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyService idempotencyService)
    {
        var request = context.Request;

        // Only apply idempotency to state-modifying operations
        if (!IsStateModifyingMethod(request.Method))
        {
            await _next(context);
            return;
        }

        // Check for Idempotency-Key header
        if (!request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
        {
            _logger.LogWarning($"Missing Idempotency-Key header for {request.Method} {request.Path}");
            await _next(context);
            return;
        }

        var idempotencyKeyValue = idempotencyKey.ToString();
        if (string.IsNullOrWhiteSpace(idempotencyKeyValue) || idempotencyKeyValue.Length > 256)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Success = false,
                Message = "Invalid Idempotency-Key. Must be 1-256 characters.",
                StatusCode = 400
            });
            return;
        }

        // Read request body to calculate hash
        request.EnableBuffering();
        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;

        var requestHash = CalculateSha256(requestBody);

        // Check if this request was already processed
        var cachedLog = await idempotencyService.CheckAsync(idempotencyKeyValue, requestHash);
        if (cachedLog != null)
        {
            _logger.LogInformation($"Returning cached response for idempotency key: {idempotencyKeyValue}");
            context.Response.StatusCode = cachedLog.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(cachedLog.ResponseBody);
            return;
        }

        // Capture response to store it
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            // Read the response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Store the response for this idempotency key
            // Only store successful responses (2xx status codes) to avoid caching errors
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                await idempotencyService.StoreAsync(
                    idempotencyKeyValue,
                    context.Response.StatusCode,
                    responseContent,
                    requestHash,
                    request.Method,
                    request.Path
                );
            }

            // Copy response to original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private bool IsStateModifyingMethod(string method)
    {
        return method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
               method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
               method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) ||
               method.Equals("PATCH", StringComparison.OrdinalIgnoreCase);
    }

    private string CalculateSha256(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}

public static class IdempotencyMiddlewareExtensions
{
    public static IApplicationBuilder UseIdempotencyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdempotencyMiddleware>();
    }
}
