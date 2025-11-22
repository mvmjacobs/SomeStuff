using MediatR;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Commands;

public record CreateItemCommand(string Name) : IRequest<ItemDto>;
