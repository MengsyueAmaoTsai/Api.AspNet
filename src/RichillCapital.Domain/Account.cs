using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Account : Entity<AccountId>
{
    private Account(
        AccountId id,
        UserId userId,
        string name,
        DateTimeOffset createdTime)
        : base(id)
    {
        UserId = userId;
        Name = name;
        CreatedTime = createdTime;
    }

    public UserId UserId { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Account> Create(
        AccountId id,
        UserId userId,
        string name,
        DateTimeOffset createdTime)
    {
        var account = new Account(
            id,
            userId,
            name,
            createdTime);

        return ErrorOr<Account>.With(account);
    }
}

public sealed class AccountId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private AccountId(string value)
        : base(value)
    {
    }

    public static Result<AccountId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrWhiteSpace(id), Error.Invalid($"'{nameof(AccountId)}' cannot be empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(AccountId)}' cannot be longer than {MaxLength} characters."))
            .Then(id => new AccountId(id));

    public static AccountId NewAccountId() => From(Guid.NewGuid().ToString()).Value;
}

public static class AccountErrors
{
    public static Error AlreadyExists(AccountId id) =>
        Error.Conflict($"An account with ID '{id}' already exists.");

    public static Error UserNotExists(UserId id) =>
        Error.NotFound($"A user with ID '{id}' was not found.");

    public static Error NotFound(AccountId id) =>
        Error.NotFound($"An account with ID '{id}' was not found.");
}

public abstract record AccountDomainEvent : DomainEvent
{
    public required AccountId AccountId { get; init; }
}

public sealed record AccountCreatedDomainEvent : AccountDomainEvent
{
}
