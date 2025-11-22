using MediatR;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Mediator.Queries;

public record GetItemsQuery : IRequest<IEnumerable<ItemDto>>;
