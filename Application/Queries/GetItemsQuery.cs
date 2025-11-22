using MediatR;
using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.Queries;

public record GetItemsQuery : IRequest<IEnumerable<ItemDto>>;
