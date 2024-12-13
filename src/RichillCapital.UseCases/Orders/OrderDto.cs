using RichillCapital.Domain;

namespace RichillCapital.UseCases.Orders;

public sealed record OrderDto
{
    public required string Id { get; init; }
    public required string AccountId { get; init; }
    public required string Symbol { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

internal static class OrderExtensions
{
    internal static OrderDto ToDto(this Order order) =>
        new()
        {
            Id = order.Id.Value,
            AccountId = order.AccountId.Value,
            Symbol = order.Symbol.Value,
            CreatedTime = order.CreatedTime,
        };
}