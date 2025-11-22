namespace SomeStuff.Application.Mediator.HandleR;

public class HandleR : IHandleR
{
    public Task<THandlerResponse> HandleAsync<THandlerResponse>(ICommand<THandlerResponse> request)
    {
        // Needs to resolve tye type
        // Find a the handler implementation for that type
        // Execute that handler

        return Task.FromResult<THandlerResponse>(default!);
    }
}
