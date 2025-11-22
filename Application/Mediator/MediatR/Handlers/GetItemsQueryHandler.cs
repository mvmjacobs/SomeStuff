using MediatR;
using SomeStuff.Application.Dtos;
using SomeStuff.Application.Mediator.MediatR.Queries;

namespace SomeStuff.Application.Mediator.MediatR.Handlers;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
{
    public Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = new[]
        {
            new ItemDto(1, "Item 1 from MediatR"),
            new ItemDto(2, "Item 2 from MediatR")
        };
        return Task.FromResult<IEnumerable<ItemDto>>(items);
    }
}
