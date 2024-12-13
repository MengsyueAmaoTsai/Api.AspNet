using RichillCapital.UseCases.Snapshots;

namespace RichillCapital.Api.Contracts.Snapshots;

public record SnapshotResponse
{
    public required string Id { get; init; }
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
            CreatedTime = dto.CreatedTime,
        };

    public static SnapshotDetailsResponse ToDetailsResponse(this SnapshotDto dto) =>
        new()
        {
            Id = dto.Id,
            CreatedTime = dto.CreatedTime,
        };
}