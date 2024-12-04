using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Queries;

public sealed record GetUserQuery : IQuery<ErrorOr<UserDto>>
{
    public required string UserId { get; init; }
}
