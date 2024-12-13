using RichillCapital.UseCases.SignalSubscriptions;

namespace RichillCapital.Api.Contracts.SignalSubscriptions;

public record SignalSubscriptionResponse
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string SignalSourceId { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record SignalSubscriptionDetailsResponse : SignalSubscriptionResponse
{
}

public static class SignalSubscriptionResponseMapping
{
    public static SignalSubscriptionResponse ToResponse(this SignalSubscriptionDto dto) =>
        new SignalSubscriptionResponse
        {
            Id = dto.Id,
            UserId = dto.UserId,
            SignalSourceId = dto.SignalSourceId,
            CreatedTime = dto.CreatedTime,
        };

    public static SignalSubscriptionDetailsResponse ToDetailsResponse(this SignalSubscriptionDto dto) =>
        new SignalSubscriptionDetailsResponse
        {
            Id = dto.Id,
            UserId = dto.UserId,
            SignalSourceId = dto.SignalSourceId,
            CreatedTime = dto.CreatedTime,
        };
}