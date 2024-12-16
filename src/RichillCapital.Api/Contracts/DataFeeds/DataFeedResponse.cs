using RichillCapital.UseCases.DataFeeds;

namespace RichillCapital.Api.Contracts.DataFeeds;

public record DataFeedResponse
{
    public required string Status { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record DataFeedDetailsResponse : DataFeedResponse
{
}

public static class DataFeedResponseMapping
{
    public static DataFeedResponse ToResponse(this DataFeedDto dataFeedDetailsResponse) =>
        new()
        {
            Status = dataFeedDetailsResponse.Status,
            CreatedTime = dataFeedDetailsResponse.CreatedTime,
        };

    public static DataFeedDetailsResponse ToDetailsResponse(this DataFeedDto dataFeedDetailsResponse) =>
        new()
        {
            Status = dataFeedDetailsResponse.Status,
            CreatedTime = dataFeedDetailsResponse.CreatedTime,
        };
}