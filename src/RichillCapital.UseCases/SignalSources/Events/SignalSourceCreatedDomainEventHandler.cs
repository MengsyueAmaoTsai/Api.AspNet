using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSources.Events;

internal sealed class SignalSourceCreatedDomainEventHandler(
    ILogger<SignalSourceCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<SignalSourceCreatedDomainEvent>
{
    public Task Handle(
        SignalSourceCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Signal source with id {SignalSourceId} has been created",
            domainEvent.SignalSourceId);

        return Task.CompletedTask;
    }
}
