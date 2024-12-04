using RichillCapital.UseCases.Users;

namespace RichillCapital.Api.Contracts.Users;

public record UserResponse
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string PasswordHash { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
}

public sealed record UserDetailsResponse : UserResponse
{
}

public static class UserResponseMapping
{
    public static UserResponse ToResponse(this UserDto dto) =>
        new()
        {
            Id = dto.Id,
            Email = dto.Email,
            Name = dto.Name,
            PasswordHash = dto.PasswordHash,
            CreatedTime = dto.CreatedTime,
        };

    public static UserDetailsResponse ToDetailsResponse(this UserDto dto) =>
        new()
        {
            Id = dto.Id,
            Email = dto.Email,
            Name = dto.Name,
            PasswordHash = dto.PasswordHash,
            CreatedTime = dto.CreatedTime,
        };
}
