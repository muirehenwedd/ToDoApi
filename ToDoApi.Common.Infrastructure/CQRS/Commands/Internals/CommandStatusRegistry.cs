using System.Collections.Generic;
using System.Net;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Common.Infrastructure.CQRS.Commands.Internals;

internal class CommandStatusRegistry : ICommandStatusRegistry
{
    private readonly IList<CommandStatusMapDelegate> _registry;

    public CommandStatusRegistry()
    {
        _registry = new List<CommandStatusMapDelegate>();
    }

    public void RegisterMapper(ICommandStatusMapper mapper)
    {
        _registry.Add(mapper.Map);
    }

    public HttpStatusCode GetStatusCode(ICommand command)
    {
        return TryGetStatusCode(command);
    }

    private HttpStatusCode TryGetStatusCode(ICommand command)
    {
        foreach (var commandStatusMapDelegate in _registry)
            try
            {
                return commandStatusMapDelegate.Invoke(command);
            }
            catch
            {
                // ignored
            }

        return HttpStatusCode.OK;
    }
}