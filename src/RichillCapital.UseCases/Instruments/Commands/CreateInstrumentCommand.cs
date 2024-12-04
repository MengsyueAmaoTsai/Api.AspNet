using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Instruments.Commands;

public sealed record CreateInstrumentCommand : ICommand<ErrorOr<InstrumentId>>
{
    public required string Symbol { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
}
