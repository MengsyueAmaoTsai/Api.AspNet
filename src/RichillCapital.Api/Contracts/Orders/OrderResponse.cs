using RichillCapital.UseCases.Orders;

namespace RichillCapital.Api.Contracts.Orders;

public record OrderResponse
{
    public required string Id { get; init; }
    public required string AccountId { get; init; }
    public required string Symbol { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record OrderDetailsResponse : OrderResponse
{
}

public static class OrderResponseMapping
{
    public static OrderResponse ToResponse(this OrderDto dto) =>
        new OrderResponse
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Symbol = dto.Symbol,
            CreatedTime = dto.CreatedTime,
        };

    public static OrderDetailsResponse ToDetailsResponse(this OrderDto dto) =>
        new OrderDetailsResponse
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Symbol = dto.Symbol,
            CreatedTime = dto.CreatedTime,
        };
}