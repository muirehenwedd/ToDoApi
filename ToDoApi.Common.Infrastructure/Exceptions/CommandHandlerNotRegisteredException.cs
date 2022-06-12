using System;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.Exceptions;
using ToDoApi.Common.Infrastructure.ErrorHandling;

namespace ToDoApi.Common.Infrastructure.Exceptions;

public class CommandHandlerNotRegisteredException : BaseException
{
    public CommandHandlerNotRegisteredException(ICommand command) : base(
        ErrorScope.CommandHandling,
        $"Unable to handle command '{command.GetType()}': respective CommandHandler is nor registered")
    {
    }

    public CommandHandlerNotRegisteredException(ICommand command, Exception innerException) : base(
        ErrorScope.CommandHandling,
        $"Unable to handle command '{command.GetType()}': respective CommandHandler is nor registered",
        innerException)
    {
    }
}