using RichillCapital.UseCases.Snapshots;

namespace RichillCapital.Api.Contracts.Snapshots;

public record SnapshotResponse
{
    public required string Id { get; init; }
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }
    public required int Latency { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record SnapshotDetailsResponse : SnapshotResponse
{
}

public static class SnapshotResponseMapping
{
    public static SnapshotResponse ToResponse(this SnapshotDto dto) =>
        new()
        {
            Id = dto.Id,
            SignalSourceId = dto.SignalSourceId,
            Time = dto.Time,
            Latency = dto.Latency,
            CreatedTime = dto.CreatedTime,
        };

    public static SnapshotDetailsResponse ToDetailsResponse(this SnapshotDto dto) =>
        new()
        {
            Id = dto.Id,
            SignalSourceId = dto.SignalSourceId,
            Time = dto.Time,
            Latency = dto.Latency,
            CreatedTime = dto.CreatedTime,
        };
}