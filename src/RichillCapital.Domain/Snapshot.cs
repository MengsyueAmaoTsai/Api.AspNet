using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;
public sealed class Snapshot : Entity<SnapshotId>
{
    private Snapshot(
        SnapshotId id,
        SignalSourceId signalSourceId,
        DateTimeOffset time,
        int latency,
        Symbol symbol,
        DateTimeOffset barTime,
        decimal lastPrice,
        DateTimeOffset createdTime)
        : base(id)
    {
        SignalSourceId = signalSourceId;
        Time = time;
        Latency = latency;
        Symbol = symbol;
        BarTime = barTime;
        LastPrice = lastPrice;
        CreatedTime = createdTime;
    }

    public SignalSourceId SignalSourceId { get; private set; }
    public DateTimeOffset Time { get; private set; }
    public int Latency { get; private set; }

    public Symbol Symbol { get; private set; }
    public DateTimeOffset BarTime { get; private set; }
    public decimal LastPrice { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Snapshot> Create(
        SnapshotId id,
        SignalSourceId signalSourceId,
        DateTimeOffset time,
        int latency,
        Symbol symbol,
        DateTimeOffset barTime,
        decimal lastPrice,
        DateTimeOffset createdTime)
    {
        var snapshot = new Snapshot(
            id,
            signalSourceId,
            time,
            latency,
            symbol,
            barTime,
            lastPrice,
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

