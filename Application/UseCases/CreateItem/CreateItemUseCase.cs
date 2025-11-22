using SomeStuff.Application.UseCases.CreateItem;

namespace SomeStuff.Application.UseCases.CreateItem;

public class CreateItemUseCase : ICreateItemUseCase
{
    public Task<object> ExecuteAsync(object request, CancellationToken cancellationToken)
    {
        // In a real-world scenario, you would process the request and
        // create a new resource in a database or another service.
        // For this example, we'll just return the request object.
        return Task.FromResult(request);
    }
}
