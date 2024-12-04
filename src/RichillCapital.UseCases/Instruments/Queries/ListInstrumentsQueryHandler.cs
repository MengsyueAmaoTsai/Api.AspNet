using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Instruments.Queries;

internal sealed class ListInstrumentsQueryHandler(
    IReadOnlyRepository<Instrument> _repository) :
    IQueryHandler<ListInstrumentsQuery, ErrorOr<IEnumerable<InstrumentDto>>>
{
    public async Task<ErrorOr<IEnumerable<InstrumentDto>>> Handle(
        ListInstrumentsQuery query,
        CancellationToken cancellationToken)
    {
        var instruments = await _repository.ListAsync(cancellationToken);

        var dtos = instruments
            .Select(instrument => instrument.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<InstrumentDto>>.With(dtos);
    }
}