using Microsoft.Extensions.Time.Testing;
using Polly.CircuitBreaker;
using SomeStuff.Infrastructure.CircuitBreaker;

namespace SomeStuff.Tests.Infrastructure.CircuitBreaker;

public sealed class PollyCircuitBreakerTests
{
    private const string Key = "Classes:BookClass";

    [Fact]
    public async Task ExecuteAsync_RunsAction_WhenCircuitIsClosed()
    {
        var breaker = new PollyCircuitBreaker();
        var executed = false;

        await breaker.ExecuteAsync(Key, _ =>
        {
            executed = true;
            return Task.CompletedTask;
        }, CancellationToken.None);

        Assert.True(executed);
    }

    [Fact]
    public async Task ExecuteAsync_AllowsRequests_WhenFailuresBelowThreshold()
    {
        var breaker = new PollyCircuitBreaker();

        // 2 failures (below threshold of 3)
        for (var i = 0; i < 2; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        var executed = false;
        await breaker.ExecuteAsync(Key, _ =>
        {
            executed = true;
            return Task.CompletedTask;
        }, CancellationToken.None);

        Assert.True(executed);
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsBrokenCircuitException_AfterThreeFailures()
    {
        var breaker = new PollyCircuitBreaker();

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        await Assert.ThrowsAsync<BrokenCircuitException>(() =>
            breaker.ExecuteAsync(Key, _ => Task.CompletedTask, CancellationToken.None));
    }

    [Fact]
    public async Task ExecuteAsync_AllowsProbeRequest_AfterBreakDurationExpires()
    {
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var breaker = new PollyCircuitBreaker(timeProvider);

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        // Advance past the 30-second break duration
        timeProvider.Advance(TimeSpan.FromSeconds(31));

        var executed = false;
        await breaker.ExecuteAsync(Key, _ =>
        {
            executed = true;
            return Task.CompletedTask;
        }, CancellationToken.None);

        Assert.True(executed);
    }

    [Fact]
    public async Task ExecuteAsync_ClosesCircuit_AfterSuccessfulProbe()
    {
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var breaker = new PollyCircuitBreaker(timeProvider);

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        // Move to half-open
        timeProvider.Advance(TimeSpan.FromSeconds(31));

        // Successful probe
        await breaker.ExecuteAsync(Key, _ => Task.CompletedTask, CancellationToken.None);

        // Circuit should be closed — multiple requests allowed
        var count = 0;
        for (var i = 0; i < 5; i++)
        {
            await breaker.ExecuteAsync(Key, _ =>
            {
                count++;
                return Task.CompletedTask;
            }, CancellationToken.None);
        }

        Assert.Equal(5, count);
    }

    [Fact]
    public async Task ExecuteAsync_ReopensCircuit_WhenProbeFailsInHalfOpen()
    {
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var breaker = new PollyCircuitBreaker(timeProvider);

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        // Move to half-open
        timeProvider.Advance(TimeSpan.FromSeconds(31));

        // Probe fails
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));

        // Circuit should be open again
        await Assert.ThrowsAsync<BrokenCircuitException>(() =>
            breaker.ExecuteAsync(Key, _ => Task.CompletedTask, CancellationToken.None));
    }

    [Fact]
    public async Task ExecuteAsync_CircuitsAreIsolatedByKey()
    {
        var breaker = new PollyCircuitBreaker();

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync("endpoint-a", _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        // endpoint-a is open
        await Assert.ThrowsAsync<BrokenCircuitException>(() =>
            breaker.ExecuteAsync("endpoint-a", _ => Task.CompletedTask, CancellationToken.None));

        // endpoint-b is unaffected
        var executed = false;
        await breaker.ExecuteAsync("endpoint-b", _ =>
        {
            executed = true;
            return Task.CompletedTask;
        }, CancellationToken.None);

        Assert.True(executed);
    }

    [Fact]
    public async Task ExecuteAsync_StaysOpen_DuringBreakDuration()
    {
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var breaker = new PollyCircuitBreaker(timeProvider);

        for (var i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                breaker.ExecuteAsync(Key, _ => throw new InvalidOperationException(), CancellationToken.None));
        }

        // Only 10 seconds in — still within the 30-second break
        timeProvider.Advance(TimeSpan.FromSeconds(10));

        await Assert.ThrowsAsync<BrokenCircuitException>(() =>
            breaker.ExecuteAsync(Key, _ => Task.CompletedTask, CancellationToken.None));
    }
}
