using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.CQRS.Commands;
using ToDoApi.Common.Infrastructure.CQRS.Commands.Internals;
using ToDoApi.Common.Infrastructure.CQRS.Internals;
using ToDoApi.Common.Infrastructure.CQRS.Queries.Internals;
using ToDoApi.Common.Infrastructure.Validation;
using ToDoApi.Common.Infrastructure.Validation.Internal;
using ToDoApi.Common.Infrastructure.WebAPI.Internals;

namespace ToDoApi.Common.Infrastructure.CQRS;

public static class Extensions
{
    public static IHostBuilder UsePayloadValidation(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(services => services
                .AddScoped<IPayloadValidator, PayloadValidator>()
            );
    }

    // ReSharper disable once InconsistentNaming
    public static IHostBuilder UseCQRS(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(services => services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<ICommandDispatcher, CommandDispatcher>()
                .AddSingleton<IQueryDispatcher, QueryDispatcher>()
                .AddSingleton<ICommandStatusRegistry, CommandStatusRegistry>(provider =>
                {
                    var registry = new CommandStatusRegistry();

                    AppDomain.CurrentDomain
                        .GetAssemblies()
                        .ToList()
                        .ForEach(assembly => assembly
                            .GetTypes()
                            .Where(t => t.GetCustomAttributes<CommandStatusMapperAttribute>().Any())
                            .ToList()
                            .ForEach(mapperType =>
                            {
                                var mapper = Activator.CreateInstance(mapperType) as ICommandStatusMapper;
                                registry.RegisterMapper(mapper);
                            })
                        );

                    return registry;
                })
            );
    }

    public static IServiceCollection AddCommandHandler<THandler>(this IServiceCollection services)
        where THandler : class
    {
        var implementationType = typeof(THandler);
        var interfaces = implementationType.GetInterfaces();

        var commandHandlerInterfaceType = interfaces
            .FirstOrDefault(i => i.Name == typeof(ICommandHandler<>).Name);

        if (commandHandlerInterfaceType != null)
        {
            services.AddScoped(commandHandlerInterfaceType, implementationType);
            return services;
        }

        var resultCommandHandlerInterfaceType =
            interfaces.FirstOrDefault(i => i.Name == typeof(IResultCommandHandler<,>).Name);

        if (resultCommandHandlerInterfaceType != null)
            services.AddScoped(resultCommandHandlerInterfaceType, implementationType);

        return services;
    }

    public static IServiceCollection AddQueryHandler<THandler>(this IServiceCollection services)
        where THandler : class
    {
        var implementationType = typeof(THandler);
        var interfaces = implementationType.GetInterfaces();

        var emptyQueryHandlerInterfaceType = interfaces
            .FirstOrDefault(i => i.Name == typeof(IEmptyQueryHandler<>).Name);

        if (emptyQueryHandlerInterfaceType != null)
        {
            services.AddScoped(emptyQueryHandlerInterfaceType, implementationType);
            return services;
        }

        var queryHandlerInterfaceType =
            interfaces.FirstOrDefault(i => i.Name == typeof(IQueryHandler<,>).Name);

        if (queryHandlerInterfaceType != null)
            services.AddScoped(queryHandlerInterfaceType, implementationType);

        return services;
    }

    public static IApplicationBuilder UseServiceEndpoints(
        this IApplicationBuilder builder,
        Action<IServiceEndpointsBuilder> configuration
    )
    {
        builder.UseRouting();

        builder.UseEndpoints(router =>
        {
            var endpointsBuilder = new EndpointsBuilder(router);
            var serviceEndpointsBuilder = new ServiceEndpointsBuilder(endpointsBuilder);
            configuration(serviceEndpointsBuilder);
        });

        return builder;
    }
}