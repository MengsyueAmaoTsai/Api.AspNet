using RichillCapital.Domain.Abstractions.Events;

using RichillCapital.SharedKernel;

namespace RichillCapital.Infrastructure.Events;

internal sealed class DefaultDomainEventDispatcher(
    IDomainEventBus _eventBus) :
    IDomainEventDispatcher
{
    public async Task DispatchAndClearDomainEvents(IEnumerable<IEntity> entities)
    {
        foreach (var entity in entities)
        {
            await DispatchAndClearDomainEvents(entity);
        }
    }

    public async Task DispatchAndClearDomainEvents(IEntity entity)
    {
        var events = entity
            .GetDomainEvents()
            .ToArray();

        entity.ClearDomainEvents();

        foreach (var domainEvent in events)
        {
            await _eventBus
                .PublishAsync(domainEvent)
                .ConfigureAwait(false);
        }
    }
}
