using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSubscriptions.Events;

internal sealed class SignalSubscriptionCreatedDomainEventHandler(
    ILogger<SignalSubscriptionCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<SignalSubscriptionCreatedDomainEvent>
{
    public Task Handle(
        SignalSubscriptionCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Signal subscription with id {SignalSubscriptionId} has been created.",
            domainEvent.SignalSubscriptionId);

        return Task.CompletedTask;
    }
}