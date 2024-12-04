using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Accounts.Queries;

internal sealed class GetAccountQueryHandler(
    IReadOnlyRepository<Account> _repository) :
    IQueryHandler<GetAccountQuery, ErrorOr<AccountDto>>
{
    public async Task<ErrorOr<AccountDto>> Handle(
        GetAccountQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = AccountId.From(query.AccountId);

        if (validationResult.IsFailure)
        {
            return ErrorOr<AccountDto>.WithError(validationResult.Error);
        }

        var id = validationResult.Value;
        var maybeAccount = await _repository.FirstOrDefaultAsync(
            a => a.Id == id,
            cancellationToken);

        if (maybeAccount.IsNull)
        {
            return ErrorOr<AccountDto>.WithError(AccountErrors.NotFound(id));
        }

        var account = maybeAccount.Value;

        return ErrorOr<AccountDto>.With(account.ToDto());
    }
}