
using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Events;

internal sealed class SnapshotCreatedDomainEventHandler(
    ILogger<SnapshotCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<SnapshotCreatedDomainEvent>
{
    public Task Handle(
        SnapshotCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Snapshot with ID '{SnapshotId}' was created.",
            domainEvent.SnapshotId);

        return Task.CompletedTask;
    }
}