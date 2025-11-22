namespace SomeStuff.Application.Mediator.HandleR;

public interface IHandleR
{
    public Task<THandlerResponse> Handle<THandlerResponse>(IHandlerRequest request);
}
