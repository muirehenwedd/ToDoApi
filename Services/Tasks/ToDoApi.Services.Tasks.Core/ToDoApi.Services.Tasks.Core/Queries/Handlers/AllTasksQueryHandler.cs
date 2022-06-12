using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Services.Tasks.Core.Queries.Responses;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Queries.Handlers;

public class AllTasksQueryHandler : IQueryHandler<AllTasksQuery, AllTasksQueryResponse>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public AllTasksQueryHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }
    public async Task<AllTasksQueryResponse> HandleAsync(AllTasksQuery query)
    {
        var tasks = (await _repository
                .GetWhere(task => task.User == _identityContext.UserId))
            .OrderByDescending(task => task.CreationTimestamp);

        return new AllTasksQueryResponse
        {
            Tasks = tasks.Select(task => new AllTasksQueryResponse.TaskInfo
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationTimestamp = task.CreationTimestamp,
                IsDone = task.IsDone
            })
        };
    }
}