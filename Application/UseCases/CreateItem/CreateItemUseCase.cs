using SomeStuff.Application.Dtos;
using SomeStuff.Application.UseCases.CreateItem;

namespace SomeStuff.Application.UseCases.CreateItem;

public class CreateItemUseCase : ICreateItemUseCase
{
    public Task<ItemDto> ExecuteAsync(ItemDto request, CancellationToken cancellationToken)
    {
        // In a real-world scenario, you would process the request and
        // create a new resource in a database or another service.
        // For this example, we'll just return the request object.
        var newItem = new ItemDto(new Random().Next(1, 100), request.Name);
        return Task.FromResult(newItem);
    }
}
