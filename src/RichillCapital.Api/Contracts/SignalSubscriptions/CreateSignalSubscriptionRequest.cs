namespace RichillCapital.Api.Contracts.SignalSubscriptions;

public sealed record CreateSignalSubscriptionRequest
{
    public required string UserId { get; init; }
    public required string SignalSourceId { get; init; }
}

public sealed record SignalSubscriptionCreatedResponse : CreatedResponse
{
}
