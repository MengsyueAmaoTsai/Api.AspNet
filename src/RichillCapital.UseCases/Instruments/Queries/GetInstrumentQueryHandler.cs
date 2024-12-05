using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Instruments.Queries;

internal sealed class GetInstrumentQueryHandler(
    IReadOnlyRepository<Instrument> _repository) :
    IQueryHandler<GetInstrumentQuery, ErrorOr<InstrumentDto>>
{
    public async Task<ErrorOr<InstrumentDto>> Handle(
        GetInstrumentQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = Symbol.From(query.Symbol);

        if (validationResult.IsFailure)
        {
            return ErrorOr<InstrumentDto>.WithError(validationResult.Error);
        }

        var symbol = validationResult.Value;

        var maybeInstrument = await _repository.FirstOrDefaultAsync(
            i => i.Symbol == symbol,
            cancellationToken);

        if (maybeInstrument.IsNull)
        {
            return ErrorOr<InstrumentDto>.WithError(InstrumentErrors.NotFound(symbol));
        }

        var instrument = maybeInstrument.Value;

        return ErrorOr<InstrumentDto>.With(instrument.ToDto());
    }
}