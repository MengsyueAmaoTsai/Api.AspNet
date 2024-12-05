using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Accounts.Events;

internal sealed class AccountCreatedDomainEventHandler(
    ILogger<AccountCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<AccountCreatedDomainEvent>
{
    public Task Handle(
        AccountCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Account with ID {AccountId} has been created.",
            domainEvent.AccountId);

        return Task.CompletedTask;
    }
}