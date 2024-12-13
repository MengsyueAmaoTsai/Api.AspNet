using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;
public sealed class Snapshot : Entity<SnapshotId>
{
    private Snapshot(
        SnapshotId id,
        DateTimeOffset createdTime)
        : base(id)
    {
        CreatedTime = createdTime;
    }

    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Snapshot> Create(
        SnapshotId id,
        DateTimeOffset createdTime)
    {
        var snapshot = new Snapshot(
            id,
            createdTime);

        snapshot.RegisterDomainEvent(new SnapshotCreatedDomainEvent
        {
            SnapshotId = id,
        });

        return ErrorOr<Snapshot>.With(snapshot);
    }
}

public sealed class SnapshotId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private SnapshotId(string value)
        : base(value)
    {
    }

    public static Result<SnapshotId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrEmpty(id), Error.Invalid($"'{nameof(SnapshotId)}' cannot be empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(SnapshotId)}' cannot be longer than {MaxLength} characters."))
            .Then(id => new SnapshotId(id));

    public static SnapshotId NewSnapshotId() =>
        From(Guid.NewGuid().ToString()).Value;
}

public static class SnapshotErrors
{
    public static Error NotFound(SnapshotId id) =>
        Error.NotFound("Snapshots.NotFound", $"Snapshot with ID '{id}' was not found.");
}

public abstract record SnapshotDomainEvent : DomainEvent
{
    public required SnapshotId SnapshotId { get; init; }
}

public sealed record SnapshotCreatedDomainEvent : SnapshotDomainEvent
{
}

