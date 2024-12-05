using RichillCapital.UseCases.SignalSources;

namespace RichillCapital.Api.Contracts.SignalSources;

public record SignalSourceResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Version { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record SignalSourceDetailsResponse : SignalSourceResponse
{
}

public static class SignalSourceResponseMapping
{
    public static SignalSourceResponse ToResponse(this SignalSourceDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Version = dto.Version,
            CreatedTime = dto.CreatedTime,
        };

    public static SignalSourceDetailsResponse ToDetailsResponse(this SignalSourceDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Version = dto.Version,
            CreatedTime = dto.CreatedTime,
        };
}