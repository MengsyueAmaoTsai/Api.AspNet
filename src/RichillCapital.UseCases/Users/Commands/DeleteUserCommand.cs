using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Commands;

public sealed record DeleteUserCommand : ICommand<Result>
{
    public required string UserId { get; init; }
}
