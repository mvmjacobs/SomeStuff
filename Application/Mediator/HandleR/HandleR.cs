namespace SomeStuff.Application.Mediator.HandleR;

public class HandleR : IHandleR
{
    public Task<THandlerResponse> HandleAsync<THandlerResponse>(IHandlerRequest<THandlerResponse> request)
    {
        return Task.FromResult<THandlerResponse>(default!);
    }
}
