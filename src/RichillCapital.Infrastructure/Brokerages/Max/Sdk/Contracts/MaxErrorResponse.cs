namespace RichillCapital.Infrastructure.Brokerages.Max.Sdk.Contracts;

public sealed record MaxErrorResponse
{
    public required MaxErrorInternalResponse Error { get; init; }
}

public sealed record MaxErrorInternalResponse
{
    public required int Code { get; init; }
    public required string Message { get; init; }
}