using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Commands;

public sealed record CreateSnapshotCommand : ICommand<ErrorOr<SnapshotId>>
{
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }
}
