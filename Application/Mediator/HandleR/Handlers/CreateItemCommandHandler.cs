using SomeStuff.Application.Dtos;
using SomeStuff.Application.Mediator.HandleR.Commands;

namespace SomeStuff.Application.Mediator.HandleR.Handlers;

public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, ItemDto>
{
    public Task<ItemDto> HandleAsync(CreateItemCommand command)
    {
        // The specific logic to create an item would go here.
        throw new NotImplementedException();
    }
}

