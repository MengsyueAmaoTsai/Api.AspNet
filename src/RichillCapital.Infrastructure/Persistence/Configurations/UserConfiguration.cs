﻿using RichillCapital.Domain;

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
                id: "UID0000001",
                email: "mengsyue.tsai@outlook.com",
                name: "RichillCapital",
                passwordHash: "among7201"),
            CreateUser(
                id: "UID0000002",
                email: "mengsyue.tsai@gmail.com",
                name: "Mengsyue Amao Tsai",
                passwordHash: "among7201"),
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
