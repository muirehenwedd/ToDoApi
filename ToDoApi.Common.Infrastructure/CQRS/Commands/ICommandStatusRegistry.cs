using System.Net;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Common.Infrastructure.CQRS.Commands;

public interface ICommandStatusRegistry
{
    public void RegisterMapper(ICommandStatusMapper mapper);

    public HttpStatusCode GetStatusCode(ICommand command);
}