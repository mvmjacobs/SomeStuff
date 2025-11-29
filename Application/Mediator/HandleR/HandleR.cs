namespace SomeStuff.Application.Mediator.HandleR;

public class HandleR : IHandleR
{
    private readonly IServiceProvider _serviceProvider;

    public HandleR(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<THandlerResponse> HandleAsync<THandlerResponse>(ICommand<THandlerResponse> command)
    {
        // Needs to resolve the type
        var commandType = command.GetType(); // it will return CreateItemCommand

        // Find the handler implementation for that type
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(THandlerResponse)); // it will return ICommandHandler<CreateItemCommand, ItemDto>
        var handlerInstance = _serviceProvider.GetRequiredService(handlerType); // it will return instance of CreateItemCommandHandler
        var handleMethod = handlerType.GetMethod(nameof(ICommandHandler<ICommand<THandlerResponse>, THandlerResponse>.HandleAsync));

        // Execute that handler
        var result = handleMethod?.Invoke(handlerInstance, [command]);
        return (Task<THandlerResponse>)result!;
    }
}
