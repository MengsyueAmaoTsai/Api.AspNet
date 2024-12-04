using RichillCapital.Domain.Abstractions.Clock;

namespace RichillCapital.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
