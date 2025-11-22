using MediatR;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Mediator.Commands;

public record CreateItemCommand(string Name) : IRequest<ItemDto>;
