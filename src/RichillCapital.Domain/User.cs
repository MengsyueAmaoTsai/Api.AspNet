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

        return ErrorOr<User>.With(user);
    }
}
