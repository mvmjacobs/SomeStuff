using MediatR;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Mediator.MediatR.Queries;

public record GetItemsQuery : IRequest<IEnumerable<ItemDto>>;
