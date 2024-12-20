namespace RichillCapital.Api.Contracts.Orders;

public sealed record CreateOrderRequest
{
    public required string AccountId { get; init; }
    public required string Symbol { get; init; }
    public required string TradeType { get; init; }
    public required string OrderType { get; init; }
    public required string TimeInForce { get; init; }
    public required decimal Quantity { get; init; }
}

public sealed record OrderCreatedResponse : CreatedResponse
{
}
