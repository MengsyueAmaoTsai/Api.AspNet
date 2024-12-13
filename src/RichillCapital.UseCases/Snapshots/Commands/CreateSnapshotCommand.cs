using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Commands;

public sealed record CreateSnapshotCommand : ICommand<ErrorOr<SnapshotId>>
{
}
