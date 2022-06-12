using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Context.Internals;

namespace ToDoApi.Common.Infrastructure.Context;

public static class Extensions
{
    public static IHostBuilder UseServiceContext(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(services => services
                .AddScoped<IServiceContextProvider, ServiceContextProvider>()
                .AddScoped(serviceProvider => serviceProvider
                    .GetRequiredService<IServiceContextProvider>()
                    .ProvideIdentityContext()
                )
            );
    }

    public static IHostBuilder UseSafeServiceContext(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(services => services
                .AddScoped<IServiceContextProvider, ServiceContextProvider>()
                .AddScoped(serviceProvider =>
                {
                    var serviceContextProvider = serviceProvider.GetRequiredService<IServiceContextProvider>();

                    try
                    {
                        return serviceContextProvider.ProvideIdentityContext();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                })
            );
    }
}