using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Tasks.Core.Commands;
using ToDoApi.Services.Tasks.Core.Commands.Handlers;
using ToDoApi.Services.Tasks.Core.Commands.Responses;
using ToDoApi.Services.Tasks.Core.Queries;
using ToDoApi.Services.Tasks.Core.Queries.Handlers;
using ToDoApi.Services.Tasks.Core.Queries.Responses;
using ToDoApi.Services.Tasks.Domain.Repository;
using ToDoApi.Services.Tasks.Infrastructure.Data;
using ToDoApi.Services.Tasks.Infrastructure.Repository;

namespace ToDoApi.Services.Tasks.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddTaskService(this IServiceCollection services)
    {
        return services
            .AddPostgres<ToDoTaskDataContext>()
            .AddScoped<ITodoTaskRepository, ToDoTaskRepository>()
            .AddCommandHandler<NewTaskCommandHandler>()
            .AddCommandHandler<ModifyTaskCommandHandler>()
            .AddQueryHandler<TaskQueryHandler>()
            .AddCommandHandler<SetDoneCommandHandler>()
            .AddCommandHandler<SetNotDoneCommandHandler>()
            .AddCommandHandler<DeleteTaskCommandHandler>()
            .AddQueryHandler<AllTasksQueryHandler>()
            .AddQueryHandler<PagedTasksQueryHandler>();
    }

    public static IApplicationBuilder UseTaskEndpoints(this IApplicationBuilder app)
    {
        app.UseServiceEndpoints(endpoints => endpoints
            .Post<NewTaskCommand, NewTaskCommandResponse>("/api/tasks/new")
            .Put<ModifyTaskCommand, ModifyTaskCommandResponse>("/api/tasks/{taskId}")
            .Get<TaskQuery, TaskQueryResponse>("/api/tasks/{taskId}")
            .Put<SetDoneCommand>("/api/tasks/{taskId}/done")
            .Put<SetNotDoneCommand>("/api/tasks/{taskId}/notDone")
            .Delete<DeleteTaskCommand>("/api/tasks/{taskId}")
            .Get<AllTasksQuery, AllTasksQueryResponse>("/api/tasks/")
            .Get<PagedTasksQuery, PagedTasksQueryResponse>("/api/tasks/paged")
        );
        return app;
    }
}