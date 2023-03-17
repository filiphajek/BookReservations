using Microsoft.AspNetCore.OutputCaching;

namespace BookReservations.Api.Caching;

public class OutputCachePolicy : IOutputCachePolicy
{
    public static readonly OutputCachePolicy Instance = new();

    private OutputCachePolicy()
    {
    }

    ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        context.CacheVaryByRules.QueryKeys = "*";
        context.ResponseExpirationTimeSpan = TimeSpan.FromSeconds(15);
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        context.AllowCacheStorage = true;
        return ValueTask.CompletedTask;
    }

    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        var request = context.HttpContext.Request;
        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
        {
            return false;
        }
        return true;
    }
}