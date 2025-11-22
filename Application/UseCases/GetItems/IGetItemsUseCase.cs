namespace SomeStuff.Application.UseCases.GetItems;

public interface IGetItemsUseCase
{
    Task<object> ExecuteAsync(CancellationToken cancellationToken);
}
