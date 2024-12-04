using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Queries;

internal sealed class ListUsersQueryHandler(
    IReadOnlyRepository<User> _userRepository) :
    IQueryHandler<ListUsersQuery, ErrorOr<IEnumerable<UserDto>>>
{
    public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(
        ListUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAsync(cancellationToken);

        var dtos = users
            .Select(u => u.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<UserDto>>.With(dtos);
    }
}
