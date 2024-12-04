using Hangfire;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RichillCapital.Infrastructure.BackgroundJobs;

public static class BackgroundJobServiceExtensions
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var databaseOptions = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(databaseOptions.ConnectionString);
        });

        services.AddHangfireServer(options =>
        {
            options.SchedulePollingInterval = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}