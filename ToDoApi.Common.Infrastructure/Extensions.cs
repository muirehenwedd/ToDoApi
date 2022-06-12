using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Attributes;
using ToDoApi.Common.Infrastructure.Exceptions;

namespace ToDoApi.Common.Infrastructure;

public static class Extensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
        where TOptions : new()
    {
        var optionsType = typeof(TOptions);
        var optionsAttribute = optionsType.GetCustomAttribute<ConfigOptionsAttribute>();

        if (optionsAttribute is null)
            throw new ConfigurationException(
                $"Unable to get options of type '{optionsType}': missing ConfigOptionsAttribute.");

        var options = new TOptions();
        configuration.GetSection(optionsAttribute.SectionName).Bind(options);

        optionsType.GetProperties().ToList().ForEach(info =>
        {
            var attribute = info.GetCustomAttribute<EnvironmentVariableAttribute>();

            if (attribute is null) return;

            var value = configuration.GetSection(attribute.Name).Value;
            info.SetValue(options, value);
        });

        return options;
    }

    public static IServiceCollection AddConfigurationOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetService<IConfiguration>();
        var options = config.GetOptions<TOptions>();
        services.AddSingleton(options);
        return services;
    }

    public static IHostBuilder ConfigureDefaults(this IHostBuilder builder, Action<IApplicationBuilder> configuration)
    {
        return builder.ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
            .Configure(app =>
            {
                app.UseRouting();
                configuration(app);
            })
        );
    }

    public static IHostBuilder Configure(this IHostBuilder builder, Action<IApplicationBuilder> configuration)
    {
        return builder.ConfigureWebHost(webHostBuilder => webHostBuilder
            .Configure(configuration)
        );
    }
}