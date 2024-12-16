namespace RichillCapital.UseCases.Brokerages;

public sealed record BrokerageDto
{
    public required string Status { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}