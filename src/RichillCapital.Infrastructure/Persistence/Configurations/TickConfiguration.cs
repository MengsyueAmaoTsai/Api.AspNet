using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class TickConfiguration : IEntityTypeConfiguration<Tick>
{
    public void Configure(EntityTypeBuilder<Tick> builder)
    {
        builder
            .HasKey(tick => new
            {
                tick.Symbol,
                tick.Price,
                tick.Volume,
                tick.Time,
            });

        builder
            .Property(tick => tick.Symbol)
            .HasMaxLength(Symbol.MaxLength)
            .HasConversion(
                symbol => symbol.Value,
                value => Symbol.From(value).ThrowIfFailure().Value)
            .IsRequired();
    }
}
