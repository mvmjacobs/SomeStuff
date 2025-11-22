using SomeStuff.Application.Dtos;
using SomeStuff.Application.UseCases.GetItems;

namespace SomeStuff.Application.UseCases.GetItems;

public class GetItemsUseCase : IGetItemsUseCase
{
    public Task<IEnumerable<ItemDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        // In a real-world scenario, you would fetch data from a database
        // or another service. For this example, we'll return a simple object.
        var data = new[] {
            new ItemDto(1, "Item 1"),
            new ItemDto(2, "Item 2")
        };
        return Task.FromResult<IEnumerable<ItemDto>>(data);
    }
}
