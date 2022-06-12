using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Queries;

public interface IQueryDispatcher
{
    public Task<TResult> DispatchAsync<TResult>();
    public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery;
}