using SomeStuff.Infrastructure.RateLimiting;

namespace SomeStuff.Tests.Infrastructure.RateLimiting;

public sealed class InMemoryBookingRateLimiterTests
{
    [Fact]
    public void TryAcquire_RejectsSixthRequestWithinWindow_ForSameUser()
    {
        var now = new DateTime(2026, 03, 14, 12, 0, 0, DateTimeKind.Utc);
        var rateLimiter = new InMemoryBookingRateLimiter(() => now);

        var attempts = Enumerable.Range(0, 6)
            .Select(_ => rateLimiter.TryAcquire("user-1", "127.0.0.1"))
            .ToArray();

        Assert.Equal(5, attempts.Count(a => a.IsAllowed));
        Assert.False(attempts[5].IsAllowed);
        Assert.True(attempts[5].RetryAfterSeconds > 0);
    }

    [Fact]
    public void TryAcquire_UsesUserIdBeforeIpAddress()
    {
        var now = new DateTime(2026, 03, 14, 12, 0, 0, DateTimeKind.Utc);
        var rateLimiter = new InMemoryBookingRateLimiter(() => now);

        foreach (var user in Enumerable.Range(0, 6).Select(i => $"user-{i}"))
        {
            var result = rateLimiter.TryAcquire(user, "127.0.0.1");
            Assert.True(result.IsAllowed);
        }
    }
}
