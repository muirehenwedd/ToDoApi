using System.Net;
using ToDoApi.Common.Core.CQRS.Commands;

namespace ToDoApi.Common.Infrastructure.CQRS.Commands;

public delegate HttpStatusCode CommandStatusMapDelegate(ICommand command);

public interface ICommandStatusMapper
{
    public HttpStatusCode Map(ICommand command);
}