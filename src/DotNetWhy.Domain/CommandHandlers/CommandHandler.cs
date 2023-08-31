namespace DotNetWhy.Domain.CommandHandlers;

internal interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    void Handle(TCommand command);
}

internal interface ICommandHandler<in TCommand, out TCommandResult>
    where TCommand : ICommand
{
    TCommandResult Handle(TCommand command);
}