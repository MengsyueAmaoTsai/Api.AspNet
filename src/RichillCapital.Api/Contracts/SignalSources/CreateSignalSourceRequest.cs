namespace RichillCapital.Api.Contracts.SignalSources;

public sealed record CreateSignalSourceRequest
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Version { get; init; }
}

public sealed record SignalSourceCreatedResponse : CreatedResponse
{
}