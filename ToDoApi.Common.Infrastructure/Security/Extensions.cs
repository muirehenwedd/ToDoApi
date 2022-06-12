using Microsoft.Extensions.Hosting;
using ToDoApi.Common.Infrastructure.Security.Options;

namespace ToDoApi.Common.Infrastructure.Security;

public static class Extensions
{
    public static IHostBuilder UseSecurity(this IHostBuilder builder)
    {
        return builder
            .ConfigureServices(service => service
                .AddConfigurationOptions<SslOptions>()
            );
    }
}