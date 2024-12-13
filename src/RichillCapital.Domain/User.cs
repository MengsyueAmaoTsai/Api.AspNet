using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class User : Entity<UserId>
{
    private User(
        UserId id,
        Email email,
        UserName name,
        string passwordHash,
        DateTimeOffset createdTime)
        : base(id)
    {
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        CreatedTime = createdTime;
    }

    public Email Email { get; private set; }
    public UserName Name { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<User> Create(
        UserId id,
        Email email,
        UserName name,
        string passwordHash,
        DateTimeOffset createdTime)
    {
        var user = new User(
            id,
            email,
            name,
            passwordHash,
            createdTime);

        user.RegisterDomainEvent(new UserCreatedDomainEvent
        {
            UserId = id,
        });

        return ErrorOr<User>.With(user);
    }
}

public sealed class UserId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private UserId(string value)
        : base(value)
    {
    }

    public static Result<UserId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrEmpty(id), Error.Invalid($"'{nameof(UserId)}' cannot be null or empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(UserId)}' cannot be longer than {MaxLength} characters."))
            .Then(id => new UserId(id));

    public static UserId NewUserId() =>
        From(Guid.NewGuid().ToString()).ThrowIfFailure().Value;
}

public sealed class Email : SingleValueObject<string>
{
    internal const int MaxLength = 256;

    private Email(string value)
        : base(value)
    {
    }

    public static Result<Email> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(email => !string.IsNullOrEmpty(email), Error.Invalid($"'{nameof(Email)}' cannot be null or empty."))
            .Ensure(email => email.Length <= MaxLength, Error.Invalid($"'{nameof(Email)}' cannot be longer than {MaxLength} characters."))
            .Then(email => new Email(email));
}

public sealed class UserName : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private UserName(string value)
        : base(value)
    {
    }

    public static Result<UserName> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(name => !string.IsNullOrEmpty(name), Error.Invalid($"'{nameof(UserName)}' cannot be null or empty."))
            .Ensure(name => name.Length <= MaxLength, Error.Invalid($"'{nameof(UserName)}' cannot be longer than {MaxLength} characters."))
            .Then(name => new UserName(name));
}

public static class UserErrors
{
    public static Error NotFound(UserId id) =>
        Error.NotFound($"Users.NotFound", $"User with id {id} was not found");

    public static Error EmailTaken(Email email) =>
        Error.Conflict($"Users.EmailTaken", $"Email {email} is already taken");
}

public abstract record UserDomainEvent : DomainEvent
{
    public required UserId UserId { get; init; }
}

public sealed record UserCreatedDomainEvent : UserDomainEvent
{
}