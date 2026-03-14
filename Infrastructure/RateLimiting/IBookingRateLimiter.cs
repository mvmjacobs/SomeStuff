namespace SomeStuff.Infrastructure.RateLimiting;

public interface IBookingRateLimiter
{
    BookingRateLimitResult TryAcquire(string? userId, string? ipAddress);
}
