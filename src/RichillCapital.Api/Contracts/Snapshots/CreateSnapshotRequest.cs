namespace RichillCapital.Api.Contracts.Snapshots;

public sealed record CreateSnapshotRequest
{
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }

    public required string Symbol { get; init; }
    public required DateTimeOffset BarTime { get; init; }
    public required decimal LastPrice { get; init; }

    public required string Message { get; init; }

    // Order snapshot
    public required string OrderTradeType { get; init; }
    public required string OrderType { get; init; }
    public required decimal OrderQuantity { get; init; }
    public required decimal OrderPrice { get; init; }

    // Position snapshot
    public required string PositionSide { get; init; }
    public required decimal PositionQuantity { get; init; }
    public required decimal PositionEntryPrice { get; init; }
    public required DateTimeOffset PositionEntryTime { get; init; }
    public required string PositionEntryComment { get; init; }
}

public sealed record SnapshotCreatedResponse : CreatedResponse
{
}