using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace TaskManagement.Backend.Middleware;

[AttributeUsage(AttributeTargets.Method)]
public class IdempotencyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Request-ID", out var requestId))
        {
            // If request ID is missing, we might allow it or fail. 
            // For this requirement (idempotency tracking), let's imply it's required for this endpoint.
            // Or we can just skip idempotency check if missing. 
            // Let's require it for robustness if the client is updated.
            base.OnActionExecuting(context);
            return;
        }

        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var cacheKey = $"Idempotency_{requestId}";

        if (cache.TryGetValue(cacheKey, out _))
        {
            context.Result = new ConflictObjectResult(new { Message = "Duplicate request detected." });
            return;
        }

        // We add to cache *after* execution usually, OR *before* to prevent concurrent processing.
        // To prevent double-submission (click twice fast), we should cache BEFORE.
        // But if the request fails, we might want to allow retry? 
        // Simple idempotency: cache immediately. 
        // Expiration: 1 hour.
        cache.Set(cacheKey, true, TimeSpan.FromHours(1));

        base.OnActionExecuting(context);
    }
}
