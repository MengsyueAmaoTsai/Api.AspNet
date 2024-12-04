using RichillCapital.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(user => user.Id);

        builder
            .HasIndex(user => user.Email);

        builder
            .Property(user => user.Id)
            .HasMaxLength(UserId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(Email.MaxLength)
            .HasConversion(
                email => email.Value,
                value => Email.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(user => user.Name)
            .HasMaxLength(UserName.MaxLength)
            .HasConversion(
                name => name.Value,
                value => UserName.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder.HasData([
            CreateUser(
                id: "1",
                email: "someone@example.com",
                name: "RichillCapital User",
                passwordHash: "PA55W0RD"),
            CreateUser(
                id: "2",
                email: "anotherone@example.com",
                name: "RichillCapital User 2",
                passwordHash: "PA55W0RD"),
        ]);
    }

    private static User CreateUser(
        string id,
        string email,
        string name,
        string passwordHash) => User
        .Create(
            UserId.From(id).ThrowIfFailure().Value,
            Email.From(email).ThrowIfFailure().Value,
            UserName.From(name).ThrowIfFailure().Value,
            passwordHash,
            DateTimeOffset.UtcNow)
        .ThrowIfError()
        .Value;
}
