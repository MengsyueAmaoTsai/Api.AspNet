using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class SignalSubscription : Entity<SignalSubscriptionId>
{
    private SignalSubscription(
        SignalSubscriptionId id,
        UserId userId,
        SignalSourceId signalSourceId,
        DateTimeOffset createdTime)
        : base(id)
    {
        UserId = userId;
        SignalSourceId = signalSourceId;
        CreatedTime = createdTime;
    }

    public UserId UserId { get; private set; }
    public SignalSourceId SignalSourceId { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<SignalSubscription> Create(
        SignalSubscriptionId id,
        UserId userId,
        SignalSourceId signalSourceId,
        DateTimeOffset createdTime)
    {
        var subscription = new SignalSubscription(
            id,
            userId,
            signalSourceId,
            createdTime);

        subscription.RegisterDomainEvent(new SignalSubscriptionCreatedDomainEvent
        {
            SignalSubscriptionId = id,
        });

        return ErrorOr<SignalSubscription>.With(subscription);
    }
}

public sealed class SignalSubscriptionId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private SignalSubscriptionId(string value) : base(value)
    {
    }

    public static Result<SignalSubscriptionId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrEmpty(id), Error.Invalid($"{nameof(SignalSubscriptionId)} cannot be empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"{nameof(SignalSubscriptionId)} cannot be longer than {MaxLength} characters."))
            .Then(id => new SignalSubscriptionId(id));

    public static SignalSubscriptionId NewSignalSubscriptionId() =>
        From(Guid.NewGuid().ToString()).Value;
}

public abstract record SignalSubscriptionDomainEvent : DomainEvent
{
    public required SignalSubscriptionId SignalSubscriptionId { get; init; }
}

public sealed record SignalSubscriptionCreatedDomainEvent : SignalSubscriptionDomainEvent
{
}

public static class SignalSubscriptionErrors
{
    public static Error NotFound(SignalSubscriptionId id) =>
        Error.NotFound("SignalSubscriptions.NotFound", $"SignalSubscription with id {id} was not found.");
}