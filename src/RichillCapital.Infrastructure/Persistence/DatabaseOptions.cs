using FluentValidation;

namespace RichillCapital.Infrastructure.Persistence;

public sealed record DatabaseOptions
{
    internal const string SectionKey = "Database";

    public required string ConnectionString { get; init; }
}

internal sealed class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithMessage("Database connection string is required.");
    }
}