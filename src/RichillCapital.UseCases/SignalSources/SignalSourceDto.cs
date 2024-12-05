using RichillCapital.Domain;

namespace RichillCapital.UseCases.SignalSources;

public sealed record SignalSourceDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Version { get; init; }
    public required string Stage { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

internal static class SignalSourceExtensions
{
    internal static SignalSourceDto ToDto(this SignalSource signalSource) =>
        new()
        {
            Id = signalSource.Id.Value,
            Name = signalSource.Name,
            Description = signalSource.Description,
            Version = signalSource.Version,
            Stage = signalSource.Stage.Name,
            CreatedTime = signalSource.CreatedTime
        };
}