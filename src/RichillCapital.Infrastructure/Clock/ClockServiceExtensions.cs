using RichillCapital.Domain.Abstractions.Clock;
using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Clock;

public static class ClockServiceExtensions
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services) =>
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
}