using RichillCapital.UseCases.Instruments;

namespace RichillCapital.Api.Contracts;

public record InstrumentResponse
{
    public required string Id { get; init; }
    public required string Symbol { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record InstrumentDetailsResponse : InstrumentResponse
{
}

public static class InstrumentResponseMapping
{
    public static InstrumentResponse ToResponse(this InstrumentDto dto) =>
        new()
        {
            Id = dto.Id,
            Symbol = dto.Symbol,
            Description = dto.Description,
            Type = dto.Type,
            CreatedTime = dto.CreatedTime,
        };

    public static InstrumentDetailsResponse ToDetailsResponse(this InstrumentDto dto) =>
        new()
        {
            Id = dto.Id,
            Symbol = dto.Symbol,
            Description = dto.Description,
            Type = dto.Type,
            CreatedTime = dto.CreatedTime,
        };
}