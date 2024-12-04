namespace RichillCapital.Domain.Abstractions.Identity;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    UserId Id { get; }
    Email Email { get; }
    UserName Name { get; }
}