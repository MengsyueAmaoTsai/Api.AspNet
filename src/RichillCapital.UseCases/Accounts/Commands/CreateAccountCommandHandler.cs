using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Accounts.Commands;

internal sealed class CreateAccountCommandHandler(
    IReadOnlyRepository<User> _userRepository,
    IDateTimeProvider _dateTimeProvider,
    IRepository<Account> _accountRepository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateAccountCommand, ErrorOr<AccountId>>
{
    public async Task<ErrorOr<AccountId>> Handle(
        CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = UserId.From(command.UserId);

        if (validationResult.IsFailure)
        {
            return ErrorOr<AccountId>.WithError(validationResult.Error);
        }

        var userId = validationResult.Value;

        if (!await _userRepository.AnyAsync(u => u.Id == userId, cancellationToken))
        {
            return ErrorOr<AccountId>.WithError(AccountErrors.UserNotExists(userId));
        }

        var errorOrAccount = Account.Create(
            AccountId.NewAccountId(),
            userId,
            command.Name,
            _dateTimeProvider.UtcNow);

        if (errorOrAccount.HasError)
        {
            return ErrorOr<AccountId>.WithError(errorOrAccount.Errors);
        }

        var account = errorOrAccount.Value;

        _accountRepository.Add(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<AccountId>.With(account.Id);
    }
}