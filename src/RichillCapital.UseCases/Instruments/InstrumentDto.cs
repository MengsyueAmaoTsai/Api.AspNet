using RichillCapital.Domain;

namespace RichillCapital.UseCases.Instruments;

public sealed record InstrumentDto
{
    public required string Id { get; init; }
    public required string Symbol { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

internal static class InstrumentExtensions
{
    internal static InstrumentDto ToDto(this Instrument instrument) =>
        new()
        {
            Id = instrument.Id.Value,
            Symbol = instrument.Symbol.Value,
            Description = instrument.Description,
            Type = instrument.Type.Name,
            CreatedTime = instrument.CreatedTime,
        };
}