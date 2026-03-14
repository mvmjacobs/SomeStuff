namespace SomeStuff.Infrastructure.RateLimiting;

public sealed record BookingRateLimitResult(bool IsAllowed, int RetryAfterSeconds = 0);
