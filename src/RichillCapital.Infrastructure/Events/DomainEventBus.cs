using RichillCapital.Domain.Abstractions.Events;

using RichillCapital.SharedKernel;

namespace RichillCapital.Infrastructure.Events
{
    internal sealed class DomainEventBus(
       InMemoryDomainEventQueue _eventQueue) :
       IDomainEventBus
    {
        public async Task PublishAsync<TDomainEvent>(
            TDomainEvent domainEvent,
            CancellationToken cancellationToken = default)
            where TDomainEvent : IDomainEvent
        {
            await _eventQueue.Writer.WriteAsync(domainEvent, cancellationToken);
        }
    }
}
