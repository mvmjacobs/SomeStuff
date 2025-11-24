namespace SomeStuff.Application.Mediator.HandleR;

public interface ICommandHandler<TCommand, THandlerResponse>
{
    public Task<THandlerResponse> HandleAsync(TCommand command);
}
