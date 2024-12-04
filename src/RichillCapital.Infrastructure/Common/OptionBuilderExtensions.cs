using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RichillCapital.Infrastructure.Common;

internal static class OptionBuilderExtensions
{
    internal static OptionsBuilder<TOptions> ValidateWithFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services
            .AddSingleton<IValidateOptions<TOptions>, FluentValidateOptions<TOptions>>(
                serviceProvider => new FluentValidateOptions<TOptions>(
                    serviceProvider,
                    builder.Name));

        return builder;
    }
}