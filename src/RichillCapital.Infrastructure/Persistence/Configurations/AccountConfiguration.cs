using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .HasKey(account => account.Id);

        builder
            .Property(account => account.Id)
            .HasMaxLength(AccountId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => AccountId.From(value).Value)
            .IsRequired();

        builder
            .Property(account => account.UserId)
            .HasMaxLength(UserId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value).Value)
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(account => account.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData([
            CreateAccount(
                id: "5011908",
                userId: "UID0000001",
                name: "Pepperstone Demo"),
            CreateAccount(
                id: "1178017",
                userId: "UID0000001",
                name: "Pepperstone Live Raw"),
        ]);
    }

    private static Account CreateAccount(
        string id,
        string userId,
        string name) => Account
        .Create(
            AccountId.From(id).ThrowIfFailure().Value,
            UserId.From(userId).ThrowIfFailure().Value,
            name,
            DateTimeOffset.UtcNow)
        .ThrowIfError()
        .Value;
}