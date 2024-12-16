namespace RichillCapital.UseCases.DataFeeds;

public sealed record DataFeedDto
{
    public required string Status { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}