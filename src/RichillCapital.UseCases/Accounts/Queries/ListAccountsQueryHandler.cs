using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Accounts.Queries;

internal sealed class ListAccountsQueryHandler(
    IReadOnlyRepository<Account> _repository) :
    IQueryHandler<ListAccountsQuery, ErrorOr<IEnumerable<AccountDto>>>
{
    public async Task<ErrorOr<IEnumerable<AccountDto>>> Handle(
        ListAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var accounts = await _repository.ListAsync(cancellationToken);

        var dtos = accounts
            .Select(a => a.ToDto())
            .ToList();

        return ErrorOr<IEnumerable<AccountDto>>.With(dtos);
    }
}
