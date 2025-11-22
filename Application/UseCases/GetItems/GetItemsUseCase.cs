using SomeStuff.Application.UseCases.GetItems;

namespace SomeStuff.Application.UseCases.GetItems;

public class GetItemsUseCase : IGetItemsUseCase
{
    public Task<object> ExecuteAsync(CancellationToken cancellationToken)
    {
        // In a real-world scenario, you would fetch data from a database
        // or another service. For this example, we'll return a simple object.
        var data = new[] {
            new { Id = 1, Name = "Item 1" },
            new { Id = 2, Name = "Item 2" }
        };
        return Task.FromResult<object>(data);
    }
}
