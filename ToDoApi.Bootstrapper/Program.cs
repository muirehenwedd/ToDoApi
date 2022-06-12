using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure;
using ToDoApi.Common.Infrastructure.Context;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Common.Infrastructure.ErrorHandling.Middleware;
using ToDoApi.Common.Infrastructure.Monitoring;
using ToDoApi.Common.Infrastructure.Monitoring.Middleware;
using ToDoApi.Common.Infrastructure.Security;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Services.Identity.Infrastructure;
using ToDoApi.Services.Tasks.Infrastructure;
using ToDoApi.Services.Identity.Infrastructure.Authorization;

namespace ToDoApi.Bootstrapper;

public class Program
{
    public static async Task Main(string[] args)
    {
        //await Task.Delay(10000);
        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseServiceContext()
            .UseCQRS()
            .UsePayloadValidation()
            .UseSecurity()
            .UseMonitoring()
            .UseVault()
            .UsePostgres()
            .ConfigureServices(services => services
                .AddIdentityService()
                .AddTaskService()
            )
            .ConfigureDefaults(app => app
                .UseMiddleware<RequestLoggingMiddleware>()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<AuthorizationMiddleware>()
                .UseHttpsRedirection()
                .UseEndpoints(endpoints =>
                    endpoints.MapGet("/health", async context =>
                    {
                        await context.WriteResponseAsync(HttpStatusCode.OK, new
                        {
                            healthy = true
                        });
                    })
                )
                .UseIdentityServiceEndpoints()
                .UseTaskEndpoints()
            );
    }
}