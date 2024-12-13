using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Queries;

internal sealed class ListSnapshotsQueryHandler(IReadOnlyRepository<Snapshot> _repository) :
    IQueryHandler<ListSnapshotsQuery, ErrorOr<IEnumerable<SnapshotDto>>>
{
    public async Task<ErrorOr<IEnumerable<SnapshotDto>>> Handle(
        ListSnapshotsQuery query,
        CancellationToken cancellationToken)
    {
        var snapshots = await _repository.ListAsync(cancellationToken);

        var dtos = snapshots
            .Select(s => s.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<SnapshotDto>>.With(dtos);
    }
}