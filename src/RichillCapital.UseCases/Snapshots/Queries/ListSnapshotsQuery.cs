using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Queries;

public sealed record ListSnapshotsQuery : IQuery<ErrorOr<IEnumerable<SnapshotDto>>>
{
}
