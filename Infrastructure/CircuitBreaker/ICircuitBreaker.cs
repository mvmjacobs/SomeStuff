namespace SomeStuff.Infrastructure.CircuitBreaker;

public interface ICircuitBreaker
{
    Task ExecuteAsync(string key, Func<CancellationToken, Task> action, CancellationToken cancellationToken);
}
