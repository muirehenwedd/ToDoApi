using System.Threading.Tasks;

namespace ToDoApi.Common.Core.CQRS.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery
{
    public Task<TResult> HandleAsync(TQuery query);
}