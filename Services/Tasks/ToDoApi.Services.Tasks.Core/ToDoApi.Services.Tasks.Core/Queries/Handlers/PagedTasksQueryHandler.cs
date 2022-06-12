using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Core.Models;
using ToDoApi.Services.Tasks.Core.Queries.Responses;
using ToDoApi.Services.Tasks.Domain.Entities;
using ToDoApi.Services.Tasks.Domain.Repository;

namespace ToDoApi.Services.Tasks.Core.Queries.Handlers;

public class PagedTasksQueryHandler : IQueryHandler<PagedTasksQuery, PagedTasksQueryResponse>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IIdentityContext _identityContext;

    public PagedTasksQueryHandler(ITodoTaskRepository repository, IIdentityContext identityContext)
    {
        _repository = repository;
        _identityContext = identityContext;
    }

    public async Task<PagedTasksQueryResponse> HandleAsync(PagedTasksQuery query)
    {
        var dataQuery = _repository.Query
            .Where(task => task.User == _identityContext.UserId)
            .OrderByDescending(task => task.CreationTimestamp);

        var pagedTasks = await Paged<ToDoTask>.ToPagedListAsync(dataQuery, query.Page, query.Top);

        return new PagedTasksQueryResponse
        {
            Top = pagedTasks.PageSize,
            Page = pagedTasks.CurrentPage,
            TotalPages = pagedTasks.TotalPages,
            Total = pagedTasks.Total,
            Tasks = pagedTasks.Select(task => new PagedTasksQueryResponse.TaskInfo
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