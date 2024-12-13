using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class SignalSubscriptionConfiguration : IEntityTypeConfiguration<SignalSubscription>
{
    public void Configure(EntityTypeBuilder<SignalSubscription> builder)
    {
        builder
            .HasKey(subscription => subscription.Id);

        builder
            .Property(subscription => subscription.Id)
            .HasMaxLength(SignalSubscriptionId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => SignalSubscriptionId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(subscription => subscription.UserId)
            .HasMaxLength(UserId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(subscription => subscription.SignalSourceId)
            .HasMaxLength(SignalSourceId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => SignalSourceId.From(value).ThrowIfFailure().Value)
            .IsRequired();
    }
}