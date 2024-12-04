using RichillCapital.SharedKernel;

namespace RichillCapital.Domain.Abstractions.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTimeOffset OccurredTime => DateTimeOffset.UtcNow;
}
