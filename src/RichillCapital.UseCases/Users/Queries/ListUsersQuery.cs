using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Queries;

public sealed record ListUsersQuery : IQuery<ErrorOr<IEnumerable<UserDto>>>
{
}
