using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.ErrorHandling;
using ToDoApi.Common.Infrastructure.ErrorHandling.Internals;
using ToDoApi.Common.Infrastructure.Monitoring.Internals;
using ToDoApi.Common.Infrastructure.Monitoring.Options;

namespace ToDoApi.Common.Infrastructure.Monitoring;

public static class Extensions
{
    public static IHostBuilder UseMonitoring(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(services => services
                .AddConfigurationOptions<LogstashOptions>()
                .AddSingleton<ISerilogConfigurationProvider, SerilogConfigurationProvider>()
                .AddSingleton<ILogger, SerilogLogger>()
                .AddSingleton<IExceptionRegistry, ExceptionRegistry>(provider =>
                {
                    var registry = new ExceptionRegistry();

                    GetByAttribute<IExceptionMapper>(typeof(ExceptionMapperAttribute))
                        .ForEach(mapper => registry.RegisterExceptionMapper(mapper));

                    return registry;
                })
            );
    }

    private static List<T> GetByAttribute<T>(Type attributeType) where T : class
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .ToList()
            .SelectMany(assembly => assembly
                .GetTypes()
                .Where(t => t.GetCustomAttributes(attributeType).Any())
                .Select(mapperType => Activator.CreateInstance(mapperType) as T)
            )
            .ToList();
    }
}