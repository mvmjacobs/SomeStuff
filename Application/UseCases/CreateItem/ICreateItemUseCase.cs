namespace SomeStuff.Application.UseCases.CreateItem;

public interface ICreateItemUseCase
{
    Task<object> ExecuteAsync(object request, CancellationToken cancellationToken);
}
