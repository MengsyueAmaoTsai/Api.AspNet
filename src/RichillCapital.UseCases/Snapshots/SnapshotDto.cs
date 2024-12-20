using RichillCapital.Domain;

namespace RichillCapital.UseCases.Snapshots;

public sealed record SnapshotDto
{
    public required string Id { get; init; }
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }
    public required int Latency { get; init; }

    public required string Symbol { get; init; }
    public required DateTimeOffset BarTime { get; init; }
    public required decimal LastPrice { get; init; }
    public required string Message { get; init; }

    public required DateTimeOffset CreatedTime { get; init; }
}

internal static class SnapshotExtensions
{
    internal static SnapshotDto ToDto(this Snapshot snapshot) =>
        new()
        {
            Id = snapshot.Id.Value,
            SignalSourceId = snapshot.SignalSourceId.Value,
            Time = snapshot.Time,
            Latency = snapshot.Latency.Value,
            Symbol = snapshot.Symbol.Value,
            BarTime = snapshot.BarTime,
            LastPrice = snapshot.LastPrice,
            Message = snapshot.Message,
            CreatedTime = snapshot.CreatedTime,
        };
}