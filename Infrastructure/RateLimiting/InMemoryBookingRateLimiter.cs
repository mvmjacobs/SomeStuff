using System.Collections.Concurrent;

namespace SomeStuff.Infrastructure.RateLimiting;

public sealed class InMemoryBookingRateLimiter : IBookingRateLimiter
{
    private const int MaxRequestsPerWindow = 5;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);
    private readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> _requests = new();
    private readonly Func<DateTime> _clock;

    public InMemoryBookingRateLimiter()
        : this(() => DateTime.UtcNow)
    {
    }

    public InMemoryBookingRateLimiter(Func<DateTime> clock)
    {
        _clock = clock;
    }

    public BookingRateLimitResult TryAcquire(string? userId, string? ipAddress)
    {
        var key = BuildKey(userId, ipAddress);
        var now = _clock();
        var queue = _requests.GetOrAdd(key, _ => new ConcurrentQueue<DateTime>());

        lock (queue)
        {
            while (queue.TryPeek(out var timestamp) && now - timestamp >= Window)
            {
                queue.TryDequeue(out _);
            }

            if (queue.Count >= MaxRequestsPerWindow)
            {
                queue.TryPeek(out var oldestRequest);
                var retryAfter = oldestRequest == default
                    ? (int)Window.TotalSeconds
                    : Math.Max(1, (int)Math.Ceiling((oldestRequest + Window - now).TotalSeconds));

                return new BookingRateLimitResult(false, retryAfter);
            }

            queue.Enqueue(now);
            return new BookingRateLimitResult(true);
        }
    }

    private static string BuildKey(string? userId, string? ipAddress)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return $"user:{userId.Trim()}";
        }

        return $"ip:{ipAddress?.Trim() ?? "unknown"}";
    }
}
