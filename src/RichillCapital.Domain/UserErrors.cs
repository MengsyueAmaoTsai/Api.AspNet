using RichillCapital.SharedKernel;

namespace RichillCapital.Domain;

public static class UserErrors
{
    public static Error NotFound(UserId id) =>
        Error.NotFound($"Users.NotFound", $"User with id {id} was not found");

    public static Error EmailTaken(Email email) =>
        Error.Conflict($"Users.EmailTaken", $"Email {email} is already taken");
}
