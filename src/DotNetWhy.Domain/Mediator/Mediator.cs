namespace DotNetWhy.Domain.Mediator;

internal interface IMediator
{
    void Send<TCommand>(
        TCommand command)
        where TCommand : ICommand;

    TCommandResult Send<TCommand, TCommandResult>(
        TCommand command)
        where TCommand : ICommand;
}

internal sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public void Send<TCommand>(
        TCommand command)
        where TCommand : ICommand =>
        _serviceProvider
            .GetRequiredService<ICommandHandler<TCommand>>()
            .Handle(command);

    public TCommandResult Send<TCommand, TCommandResult>(
        TCommand command)
        where TCommand : ICommand =>
        _serviceProvider
            .GetRequiredService<ICommandHandler<TCommand, TCommandResult>>()
            .Handle(command);
}