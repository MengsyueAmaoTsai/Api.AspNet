namespace RichillCapital.Api.Contracts.Accounts;

public sealed record CreateAccountRequest
{
    public required string UserId { get; init; }
    public required string Name { get; init; }
}

public sealed record AccountCreatedResponse : CreatedResponse
{
}