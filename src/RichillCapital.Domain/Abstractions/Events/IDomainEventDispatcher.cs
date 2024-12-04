using RichillCapital.SharedKernel;

namespace RichillCapital.Domain.Abstractions.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearDomainEvents(IEntity entity);
    Task DispatchAndClearDomainEvents(IEnumerable<IEntity> entities);
}
