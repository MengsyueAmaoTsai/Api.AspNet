namespace RichillCapital.Api.Contracts.Users;

public sealed record CreateUserRequest
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}

public sealed record UserCreatedResponse : CreatedResponse
{
}
