namespace RichillCapital.Api.Contracts.Instruments;

public sealed record CreateInstrumentRequest 
{
    public required string Symbol { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
}

public sealed record InstrumentCreatedResponse : CreatedResponse
{
}