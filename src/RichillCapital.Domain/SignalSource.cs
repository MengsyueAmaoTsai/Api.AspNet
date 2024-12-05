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
        SignalSourceStage stage,
        DateTimeOffset createdTime)
        : base(id)
    {
        Name = name;
        Description = description;
        Version = version;
        Stage = stage;
        CreatedTime = createdTime;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Version { get; private set; }
    public SignalSourceStage Stage { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<SignalSource> Create(
        SignalSourceId id,
        string name,
        string description,
        string version,
        SignalSourceStage stage,
        DateTimeOffset createdTime)
    {
        var source = new SignalSource(
            id,
            name,
            description,
            version,
            stage,
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

public sealed class SignalSourceStage : Enumeration<SignalSourceStage>
{
    public static readonly SignalSourceStage Development = new(nameof(Development), 1);
    public static readonly SignalSourceStage BackTesting = new(nameof(BackTesting), 2);
    public static readonly SignalSourceStage Simulation = new(nameof(Simulation), 3);
    public static readonly SignalSourceStage Production = new(nameof(Production), 4);
    public static readonly SignalSourceStage Paused = new(nameof(Paused), 5);
    public static readonly SignalSourceStage Deprecated = new(nameof(Deprecated), 6);

    private SignalSourceStage(string name, int value)
        : base(name, value)
    {
    }
}

public static class SignalSourceErrors
{
    public static Error NotFound(SignalSourceId id) =>
        Error.NotFound($"Signal source with id '{id}' was not found.");

    public static Error AlreadyExists(SignalSourceId id) =>
        Error.Conflict($"Signal source with id '{id}' already exists.");
}

public abstract record SignalSourceDomainEvent : DomainEvent
{
    public required SignalSourceId SignalSourceId { get; init; }
}

public sealed record SignalSourceCreatedDomainEvent : SignalSourceDomainEvent
{
}