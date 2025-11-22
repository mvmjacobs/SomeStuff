namespace SomeStuff.Application.Mediator.HandleR;

public class HandleR : IHandleR
{
    public Task<THandlerResponse> Handle<THandlerResponse>(IHandlerRequest request)
    {
        return Task.FromResult<THandlerResponse>(default!);
    }
}
