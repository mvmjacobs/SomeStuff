using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.UseCases.CreateItem;

public interface ICreateItemUseCase
{
    Task<ItemDto> ExecuteAsync(ItemDto request, CancellationToken cancellationToken);
}
