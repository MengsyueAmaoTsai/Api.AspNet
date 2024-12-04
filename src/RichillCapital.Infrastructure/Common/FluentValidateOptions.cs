using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RichillCapital.Infrastructure.Common;

internal sealed class FluentValidateOptions<TOptions>(
    IServiceProvider _serviceProvider,
    string? _name) :
    IValidateOptions<TOptions>
    where TOptions : class
{
    public ValidateOptionsResult Validate(
        string? name,
        TOptions options)
    {
        if (_name is not null && _name != name)
        {
            return ValidateOptionsResult.Skip;
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        using var scope = _serviceProvider.CreateScope();

        var validator = scope.ServiceProvider
            .GetRequiredService<IValidator<TOptions>>();

        var result = validator.Validate(options);

        if (!result.IsValid)
        {
            var type = options.GetType().Name;

            var errors = result.Errors
                .Select(error => $"Validation failed for {type}.{error.PropertyName} with the error: {error.ErrorMessage}");

            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}