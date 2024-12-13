using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class SnapshotConfiguration : IEntityTypeConfiguration<Snapshot>
{
    public void Configure(EntityTypeBuilder<Snapshot> builder)
    {
        builder
            .HasKey(snapshot => snapshot.Id);

        builder
            .Property(snapshot => snapshot.Id)
            .HasMaxLength(SnapshotId.MaxLength)
            .HasConversion(
                snapshotId => snapshotId.Value,
                value => SnapshotId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.SignalSourceId)
            .HasMaxLength(SignalSourceId.MaxLength)
            .HasConversion(
                signalSourceId => signalSourceId.Value,
                value => SignalSourceId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.Symbol)
            .HasMaxLength(Symbol.MaxLength)
            .HasConversion(
                symbol => symbol.Value,
                value => Symbol.From(value).ThrowIfFailure().Value)
            .IsRequired();
    }
}