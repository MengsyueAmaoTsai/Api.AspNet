using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Instruments.Events;

internal sealed class InstrumentCreatedDomainEventHandler(
    ILogger<InstrumentCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<InstrumentCreatedDomainEvent>
{
    public Task Handle(
        InstrumentCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Instrument with ID {InstrumentId} has been created.",
            domainEvent.InstrumentId);

        return Task.CompletedTask;
    }
}