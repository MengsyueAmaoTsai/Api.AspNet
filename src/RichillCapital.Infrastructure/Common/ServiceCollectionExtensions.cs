using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsWithFluentValidation<TOptions>(
        this IServiceCollection services,
        string sectionKey)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionKey)
            .ValidateWithFluentValidation()
            .ValidateOnStart();

        return services;
    }
}