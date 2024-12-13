using RichillCapital.Domain;

namespace RichillCapital.UseCases.SignalSubscriptions;

public sealed record SignalSubscriptionDto
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

internal static class SignalSubscriptionExtensions
{
    internal static SignalSubscriptionDto ToDto(this SignalSubscription subscription) =>
        new()
        {
            Id = subscription.Id.Value,
            UserId = subscription.UserId.Value,
            SignalSourceId = subscription.SignalSourceId.Value,
            CreatedTime = subscription.CreatedTime,
        };
}