using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

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
            .Ensure(id => !string.IsNullOrEmpty(id), Error.Invalid($"'{nameof(value)}' cannot be null or empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(value)}' cannot be longer than {MaxLength} characters."))
            .Then(id => new UserId(id));

    public static UserId NewUserId() =>
        From(Guid.NewGuid().ToString()).ThrowIfFailure().Value;
}