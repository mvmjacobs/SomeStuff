using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.UseCases.GetItems;

public interface IGetItemsUseCase
{
    Task<IEnumerable<ItemDto>> ExecuteAsync(CancellationToken cancellationToken);
}
