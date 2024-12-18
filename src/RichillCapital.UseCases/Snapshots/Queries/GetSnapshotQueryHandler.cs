using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Queries;

internal sealed class GetSnapshotQueryHandler(
    IReadOnlyRepository<Snapshot> _repository) :
    IQueryHandler<GetSnapshotQuery, ErrorOr<SnapshotDto>>
{
    public async Task<ErrorOr<SnapshotDto>> Handle(
        GetSnapshotQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = SnapshotId.From(query.SnapshotId);

        if (validationResult.IsFailure)
        {
            return ErrorOr<SnapshotDto>.WithError(validationResult.Error);
        }

        var snapshotId = validationResult.Value;

        var maybeSnapshot = await _repository.FirstOrDefaultAsync(
            snapshot => snapshot.Id == snapshotId,
            cancellationToken);

        if (maybeSnapshot.IsNull)
        {
            return ErrorOr<SnapshotDto>.WithError(SnapshotErrors.NotFound(snapshotId));
        }

        var snapshot = maybeSnapshot.Value;

        return ErrorOr<SnapshotDto>.With(snapshot.ToDto());
    }
}