using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSubscriptions.Queries;

internal sealed class ListSignalSubscriptionsQueryHandler(
    IReadOnlyRepository<SignalSubscription> _repository) :
    IQueryHandler<ListSignalSubscriptionsQuery, ErrorOr<IEnumerable<SignalSubscriptionDto>>>
{
    public async Task<ErrorOr<IEnumerable<SignalSubscriptionDto>>> Handle(
        ListSignalSubscriptionsQuery query,
        CancellationToken cancellationToken)
    {
        var subscriptions = await _repository.ListAsync(cancellationToken);

        var dtos = subscriptions
            .Select(s => s.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<SignalSubscriptionDto>>.With(dtos);
    }
}