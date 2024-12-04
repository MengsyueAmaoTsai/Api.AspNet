using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Instruments.Commands;

internal sealed class CreateInstrumentCommandHandler(
    IDateTimeProvider _dateTimeProvider,
    IRepository<Instrument> _repository,
    IUnitOfWork _unitOfWork) : 
    ICommandHandler<CreateInstrumentCommand, ErrorOr<InstrumentId>>
{
    public async Task<ErrorOr<InstrumentId>> Handle(
        CreateInstrumentCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = Result<(InstrumentId, Symbol, InstrumentType)>.Combine(
            InstrumentId.From(command.Symbol),
            Symbol.From(command.Symbol),
            InstrumentType.FromName(command.Type).ToResult(error: Error.Invalid($"Invalid type '{command.Type}'")));

        if (validationResult.IsFailure)
        {
            return ErrorOr<InstrumentId>.WithError(validationResult.Error);
        }

        var (id, symbol, type) = validationResult.Value;

        var createdTime = _dateTimeProvider.UtcNow;

        var errorOrInstrument = Instrument.Create(
            id: id,
            symbol: symbol,
            description: command.Description,
            type: type,
            createdTime: createdTime);

        if (errorOrInstrument.HasError)
        {
            return ErrorOr<InstrumentId>.WithError(errorOrInstrument.Errors);
        }

        var instrument = errorOrInstrument.Value;

        _repository.Add(instrument);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<InstrumentId>.With(instrument.Id);
    }
}