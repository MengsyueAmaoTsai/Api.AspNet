using RichillCapital.UseCases.Accounts;

namespace RichillCapital.Api.Contracts.Accounts;

public record AccountResponse
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record AccountDetailsResponse : AccountResponse
{
}

public static class AccountResponseMapping
{
    public static AccountResponse ToResponse(this AccountDto dto) =>
        new()
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Name = dto.Name,
            CreatedTime = dto.CreatedTime,
        };

    public static AccountDetailsResponse ToDetailsResponse(this AccountDto dto) =>
        new()
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Name = dto.Name,
            CreatedTime = dto.CreatedTime,
        };
}