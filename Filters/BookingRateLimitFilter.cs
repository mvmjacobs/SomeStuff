using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SomeStuff.Application.Dtos;
using SomeStuff.Infrastructure.RateLimiting;

namespace SomeStuff.Filters;

public sealed class BookingRateLimitFilter : IAsyncActionFilter
{
    private readonly IBookingRateLimiter _rateLimiter;

    public BookingRateLimitFilter(IBookingRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.ActionArguments.TryGetValue("request", out var requestArgument);
        var request = requestArgument as BookingRequestDto;
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        var result = _rateLimiter.TryAcquire(request?.UserId, ipAddress);
        if (!result.IsAllowed)
        {
            context.HttpContext.Response.Headers.RetryAfter = result.RetryAfterSeconds.ToString();
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                Content = "Too many booking attempts. Please try again later."
            };
            return;
        }

        await next();
    }
}
