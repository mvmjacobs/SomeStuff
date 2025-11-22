using MediatR;
using SomeStuff.Application.Commands;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Handlers;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    public Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        // In a real-world scenario, you would save the item to a database
        // and return the created item.
        var newItem = new ItemDto(new Random().Next(1, 100), request.Name);
        return Task.FromResult(newItem);
    }
}
