using RichillCapital.UseCases.Brokerages;

namespace RichillCapital.Api.Contracts.Brokerages;

public record BrokerageResponse
{
    public required string Status { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record BrokerageDetailsResponse : BrokerageResponse
{
}

public static class BrokerageResponseMapping
{
    public static BrokerageResponse ToResponse(this BrokerageDto dto) =>
        new()
        {
            Status = dto.Status,
            CreatedTime = dto.CreatedTime
        };

    public static BrokerageDetailsResponse ToDetailsResponse(this BrokerageDto dto) =>
        new()
        {
            Status = dto.Status,
            CreatedTime = dto.CreatedTime
        };
}