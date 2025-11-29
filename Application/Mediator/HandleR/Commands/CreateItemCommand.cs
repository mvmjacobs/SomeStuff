using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Mediator.HandleR.Commands;

public record CreateItemCommand(string Name) : ICommand<ItemDto>;
