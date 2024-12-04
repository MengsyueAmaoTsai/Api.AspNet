using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

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
            .Ensure(name => !string.IsNullOrEmpty(name), Error.Invalid($"'{nameof(value)}' cannot be null or empty."))
            .Ensure(name => name.Length <= MaxLength, Error.Invalid($"'{nameof(value)}' cannot be longer than {MaxLength} characters."))
            .Then(name => new UserName(name));
}