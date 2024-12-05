using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSources.Queries;

internal sealed class ListSignalSourcesQueryHandler(
    IReadOnlyRepository<SignalSource> _repository) :
    IQueryHandler<ListSignalSourcesQuery, ErrorOr<IEnumerable<SignalSourceDto>>>
{
    public async Task<ErrorOr<IEnumerable<SignalSourceDto>>> Handle(
        ListSignalSourcesQuery query,
        CancellationToken cancellationToken)
    {
        var sources = await _repository.ListAsync(cancellationToken);

        var dtos = sources
            .Select(s => s.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<SignalSourceDto>>.With(dtos);
    }
}
