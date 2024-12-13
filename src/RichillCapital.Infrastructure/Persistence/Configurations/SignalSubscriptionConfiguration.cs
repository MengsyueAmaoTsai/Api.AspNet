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

        builder.HasData([
            CreateSignalSubscription(
                id: "1",
                userId: "UID0000001",
                signalSourceId: "TV-DEMO-LONG"),
            CreateSignalSubscription(
                id: "2",
                userId: "UID0000001",
                signalSourceId: "TV-DEMO-SHORT"),
        ]);
    }

    private static SignalSubscription CreateSignalSubscription(
        string id,
        string userId,
        string signalSourceId) => SignalSubscription
        .Create(
            id: SignalSubscriptionId.From(id).ThrowIfFailure().Value,
            userId: UserId.From(userId).ThrowIfFailure().Value,
            signalSourceId: SignalSourceId.From(signalSourceId).ThrowIfFailure().Value,
            createdTime: DateTimeOffset.UtcNow)
        .ThrowIfError()
        .Value;
}