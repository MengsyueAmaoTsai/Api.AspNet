using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Users.Events;

internal sealed class UserCreatedDomainEventHandler(
    ILogger<UserCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<UserCreatedDomainEvent>
{
    public Task Handle(
        UserCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("User created with id: {UserId}", domainEvent.UserId);

        return Task.CompletedTask;
    }
}