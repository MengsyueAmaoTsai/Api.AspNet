using RichillCapital.SharedKernel;

namespace RichillCapital.Domain;

public static class AccountErrors
{
    public static Error AlreadyExists(AccountId id) =>
        Error.Conflict($"An account with ID '{id}' already exists.");

    public static Error UserNotExists(UserId id) =>
        Error.NotFound($"A user with ID '{id}' was not found.");

    public static Error NotFound(AccountId id) =>
        Error.NotFound($"An account with ID '{id}' was not found.");
}