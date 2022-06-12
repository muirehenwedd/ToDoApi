using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Data.Postgres.Exceptions;
using ToDoApi.Common.Infrastructure.Data.Postgres.Internals;
using ToDoApi.Common.Infrastructure.Data.Postgres.Options;
using ToDoApi.Common.Infrastructure.Vault;
using ToDoApi.Common.Utility.Extensions;

namespace ToDoApi.Common.Infrastructure.Data.Postgres;

public static class Extensions
{
    public static IHostBuilder UsePostgres(this IHostBuilder builder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return builder
            .ConfigureServices(services => services
                .AddConfigurationOptions<PostgresOptions>()
                .AddTransient<IPostgresConnectionStringProvider, PostgresConnectionStringProvider>()
            );
    }

    public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : ServiceDbContext
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretProvider = serviceProvider.GetRequiredService<IKeyValueSecretProvider>();

        var config = secretProvider.GetAsync<PostgresConnectionOptions>().GetAwaiter().GetResult();

        return services.AddDbContextPool<T>((provider, options) =>
        {
            var connectionStringProvider = provider.GetService<IPostgresConnectionStringProvider>();

            if (connectionStringProvider is null)
                throw new PostgresConfigurationException(
                    $"Unable to add DB context of type '{typeof(T)}': connection string provider is inaccessible.");

            options.UseNpgsql(connectionStringProvider.ConnectionString);
        }, config.MaxPoolSize);
    }

    public static PropertyBuilder<TProperty> PropertyColumn<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    )
        where TEntity : class
    {
        var memberExpression = propertyExpression.Body as MemberExpression;
        var propertyInfo = memberExpression?.Member as PropertyInfo;

        if (propertyInfo == null)
            throw new ArgumentException("The property name could not be acquired.");

        return builder
            .Property(propertyExpression)
            .HasColumnName(propertyInfo.Name.ToSnakeCase());
    }
}