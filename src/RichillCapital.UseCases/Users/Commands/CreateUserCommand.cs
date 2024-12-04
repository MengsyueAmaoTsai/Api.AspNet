using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Commands;

public sealed record CreateUserCommand : ICommand<ErrorOr<UserId>>
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}
