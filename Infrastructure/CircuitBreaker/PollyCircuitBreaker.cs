using System.Collections.Concurrent;
using Polly;
using Polly.CircuitBreaker;

namespace SomeStuff.Infrastructure.CircuitBreaker;

public sealed class PollyCircuitBreaker(TimeProvider timeProvider) : ICircuitBreaker
{
    private readonly ConcurrentDictionary<string, ResiliencePipeline> _pipelines = new();
    private readonly TimeProvider _timeProvider = timeProvider;

    public PollyCircuitBreaker() : this(TimeProvider.System) { }

    public async Task ExecuteAsync(string key, Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        var pipeline = _pipelines.GetOrAdd(key, _ => BuildPipeline());

        await pipeline.ExecuteAsync(
            static (state, ct) => new ValueTask(state(ct)),
            action,
            cancellationToken);
    }

    private ResiliencePipeline BuildPipeline()
    {
        return new ResiliencePipelineBuilder { TimeProvider = _timeProvider }
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 10,
                BreakDuration = TimeSpan.FromSeconds(30),
            })
            .Build();
    }
}
