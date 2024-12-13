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
    }
}