using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

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
            .Ensure(email => !string.IsNullOrEmpty(email), Error.Invalid($"'{nameof(value)}' cannot be null or empty."))
            .Ensure(email => email.Length <= MaxLength, Error.Invalid($"'{nameof(value)}' cannot be longer than {MaxLength} characters."))
            .Then(email => new Email(email));
}