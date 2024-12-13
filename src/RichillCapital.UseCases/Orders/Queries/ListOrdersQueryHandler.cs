using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Orders.Queries;

internal sealed class ListOrdersQueryHandler(
    IReadOnlyRepository<Order> _repository) :
    IQueryHandler<ListOrdersQuery, ErrorOr<IEnumerable<OrderDto>>>
{
    public async Task<ErrorOr<IEnumerable<OrderDto>>> Handle(
        ListOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.ListAsync(cancellationToken);

        var dtos = orders
            .Select(o => o.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<OrderDto>>.With(dtos);
    }
}
