using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Common.Infrastructure.CQRS;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Identity.Core.Commands;
using ToDoApi.Services.Identity.Core.Commands.Handlers;
using ToDoApi.Services.Identity.Core.Commands.Responses;
using ToDoApi.Services.Identity.Core.Queries;
using ToDoApi.Services.Identity.Core.Queries.Handlers;
using ToDoApi.Services.Identity.Core.Queries.Responses;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Domain.Repository;
using ToDoApi.Services.Identity.Infrastructure.Data;
using ToDoApi.Services.Identity.Infrastructure.Repository;
using ToDoApi.Services.Identity.Infrastructure.Services;

namespace ToDoApi.Services.Identity.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services)
    {
        return services
            .AddPostgres<IdentityDataContext>()
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<ICryptoService,CryptoService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddCommandHandler<RegistrationCommandHandler>()
            .AddCommandHandler<SignInCommandHandler>()
            .AddCommandHandler<SignOutCommandHandler>()
            .AddCommandHandler<DeleteAccountCommandHandler>()
            .AddQueryHandler<FreshTokenAcquireQueryHandler>();
    }

    public static IApplicationBuilder UseIdentityServiceEndpoints(this IApplicationBuilder app)
    {
        app.UseServiceEndpoints(endpoints => endpoints
            .Post<RegistrationCommand, RegistrationCommandResponse>("/api/identity/signup/")
            .Post<SignInCommand, SignInCommandResponse>("/api/identity/signin/")
            .Get<FreshTokenAcquireQuery, FreshTokenAcquireQueryResponse>("/api/identity/token")
            .Delete<SignOutCommand>("/api/identity/signout")
            .Delete<DeleteAccountCommand>("/api/identity/DeleteAccount")
        );
        return app;
    }
}