using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class SignalSource : Entity<SignalSourceId>
{
    private SignalSource(
        SignalSourceId id,
        string name,
        string description,
        string version,
        DateTimeOffset createdTime)
        : base(id)
    {
        Name = name;
        Description = description;
        Version = version;
        CreatedTime = createdTime;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Version { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<SignalSource> Create(
        SignalSourceId id,
        string name,
        string description,
        string version,
        DateTimeOffset createdTime)
    {
        var source = new SignalSource(
            id,
            name,
            description,
            version,
            createdTime);

        source.RegisterDomainEvent(new SignalSourceCreatedDomainEvent
        {
            SignalSourceId = id,
        });

        return ErrorOr<SignalSource>.With(source);
    }
}

public sealed class SignalSourceId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private SignalSourceId(string value)
        : base(value)
    {
    }

    public static Result<SignalSourceId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrWhiteSpace(id), Error.Invalid($"'{nameof(SignalSourceId)}' cannot be empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(SignalSourceId)}' cannot be longer than {MaxLength} characters."))
            .Then(id => new SignalSourceId(id));
}

public abstract record SignalSourceDomainEvent : DomainEvent
{
    public required SignalSourceId SignalSourceId { get; init; }
}

public sealed record SignalSourceCreatedDomainEvent : SignalSourceDomainEvent
{
}