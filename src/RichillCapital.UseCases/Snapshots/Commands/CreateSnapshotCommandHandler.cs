using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Commands;

internal sealed class CreateSnapshotCommandHandler(
    IDateTimeProvider _dateTimeProvider,
    IRepository<Snapshot> _snapshotRepository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateSnapshotCommand, ErrorOr<SnapshotId>>
{
    public async Task<ErrorOr<SnapshotId>> Handle(
        CreateSnapshotCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = Result<(SignalSourceId, Symbol)>
            .Combine(
                SignalSourceId.From(command.SignalSourceId),
                Symbol.From(command.Symbol));

        if (validationResult.IsFailure)
        {
            return ErrorOr<SnapshotId>.WithError(validationResult.Error);
        }

        var (sourceId, symbol) = validationResult.Value;
        var createdTime = _dateTimeProvider.UtcNow;

        var latency = (int)(createdTime - command.Time).TotalMilliseconds;

        var errorOrSnapshot = Snapshot.Create(
            id: SnapshotId.NewSnapshotId(),
            signalSourceId: sourceId,
            time: command.Time,
            latency: latency,
            symbol: symbol,
            barTime: command.BarTime,
            lastPrice: command.LastPrice,
            createdTime: _dateTimeProvider.UtcNow);

        if (errorOrSnapshot.HasError)
        {
            return ErrorOr<SnapshotId>.WithError(errorOrSnapshot.Errors);
        }

        var snapshot = errorOrSnapshot.Value;

        _snapshotRepository.Add(snapshot);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<SnapshotId>.With(snapshot.Id);
    }
}