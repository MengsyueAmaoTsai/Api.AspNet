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
        Latency latency,
        Symbol symbol,
        DateTimeOffset barTime,
        decimal lastPrice,
        string message,
        DateTimeOffset createdTime)
        : base(id)
    {
        SignalSourceId = signalSourceId;
        Time = time;
        Latency = latency;
        Symbol = symbol;
        BarTime = barTime;
        LastPrice = lastPrice;
        Message = message;
        CreatedTime = createdTime;
    }

    public SignalSourceId SignalSourceId { get; private set; }
    public DateTimeOffset Time { get; private set; }
    public Latency Latency { get; private set; }

    public Symbol Symbol { get; private set; }
    public DateTimeOffset BarTime { get; private set; }
    public decimal LastPrice { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Snapshot> Create(
        SnapshotId id,
        SignalSourceId signalSourceId,
        DateTimeOffset time,
        Latency latency,
        Symbol symbol,
        DateTimeOffset barTime,
        decimal lastPrice,
        string message,
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
            message,
            createdTime);

        snapshot.RegisterDomainEvent(new SnapshotCreatedDomainEvent
        {
            SnapshotId = id,
            SignalSourceId = signalSourceId,
            Time = time,
            Latency = latency,
            Symbol = symbol,
            BarTime = barTime,
            LastPrice = lastPrice,
            Message = message,
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

public sealed class Latency : SingleValueObject<int>
{
    private const int Mean = 2500;
    private const int DefaultStandardDeviation = 2500;

    private Latency(int value)
        : base(value)
    {
    }

    public static Result<Latency> Create(int value)
    {
        if (value < 0)
        {
            return Result<Latency>.Failure(Error.Invalid($"{nameof(Latency)} cannot be negative."));
        }

        var latency = new Latency(value);
        return Result<Latency>.With(latency);
    }
}

public static class SnapshotErrors
{
    public static Error NotFound(SnapshotId id) =>
        Error.NotFound("Snapshots.NotFound", $"Snapshot with ID '{id}' was not found.");

    public static Error SourceNotExists(SignalSourceId id) =>
        Error.NotFound("Snapshots.SourceNotExists", $"Signal source with ID '{id}' not exists.");
}

public abstract record SnapshotDomainEvent : DomainEvent
{
    public required SnapshotId SnapshotId { get; init; }
}

public sealed record SnapshotCreatedDomainEvent : SnapshotDomainEvent
{
    public required SignalSourceId SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }
    public required Latency Latency { get; init; }
    public required Symbol Symbol { get; init; }
    public required DateTimeOffset BarTime { get; init; }
    public required decimal LastPrice { get; init; }
    public required string Message { get; init; }
}

