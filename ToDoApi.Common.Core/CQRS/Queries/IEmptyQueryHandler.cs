using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Queries;

public interface IEmptyQueryHandler<TResult>
{
    public Task<TResult> HandleAsync();
}