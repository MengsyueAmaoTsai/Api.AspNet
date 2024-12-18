using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Queries;

public sealed record GetSnapshotQuery :
    IQuery<ErrorOr<SnapshotDto>>
{
    public required string SnapshotId { get; init; }
}
