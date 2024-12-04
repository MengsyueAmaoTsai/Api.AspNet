using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Accounts.Commands;

public sealed record CreateAccountCommand : ICommand<ErrorOr<AccountId>>
{
    public required string UserId { get; init; }
    public required string Name { get; init; }
}
