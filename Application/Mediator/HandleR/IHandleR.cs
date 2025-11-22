namespace SomeStuff.Application.Mediator.HandleR;

public interface IHandleR
{
    public Task<THandlerResponse> HandleAsync<THandlerResponse>(ICommand<THandlerResponse> command);
}
