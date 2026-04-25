using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Polly.CircuitBreaker;
using SomeStuff.Application.Dtos;
using SomeStuff.Infrastructure.CircuitBreaker;
using SomeStuff.Infrastructure.RateLimiting;

namespace SomeStuff.Filters;

public sealed class BookingRateLimitFilter(IBookingRateLimiter rateLimiter, ICircuitBreaker circuitBreaker) : IAsyncActionFilter
{
    private const string CircuitBreakerKey = "booking";
    private readonly IBookingRateLimiter _rateLimiter = rateLimiter;
    private readonly ICircuitBreaker _circuitBreaker = circuitBreaker;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.ActionArguments.TryGetValue("request", out var requestArgument);
        var request = requestArgument as BookingRequestDto;
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        try
        {
            await _circuitBreaker.ExecuteAsync(CircuitBreakerKey, async _ =>
            {
                var result = _rateLimiter.TryAcquire(request?.UserId, ipAddress);
                if (!result.IsAllowed)
                {
                    context.HttpContext.Response.Headers.RetryAfter = result.RetryAfterSeconds.ToString();
                    throw new RateLimitRejectedException(result.RetryAfterSeconds);
                }

                await next();
            }, context.HttpContext.RequestAborted);
        }
        catch (RateLimitRejectedException)
        {
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                Content = "Too many booking attempts. Please try again later."
            };
        }
        catch (BrokenCircuitException)
        {
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable,
                Content = "Service temporarily unavailable. Please try again later."
            };
        }
    }
}

public sealed class RateLimitRejectedException(int retryAfterSeconds) : Exception
{
    public int RetryAfterSeconds { get; } = retryAfterSeconds;
}
