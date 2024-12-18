using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Commands;

public sealed record CreateSnapshotCommand : ICommand<ErrorOr<SnapshotId>>
{
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset Time { get; init; }

    public required string Symbol { get; init; }
    public required DateTimeOffset BarTime { get; init; }
    public required decimal LastPrice { get; init; }
    public required string Message { get; init; }
}
