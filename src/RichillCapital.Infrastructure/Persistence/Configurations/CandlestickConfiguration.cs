using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class CandlestickConfiguration : IEntityTypeConfiguration<Candlestick>
{
    public void Configure(EntityTypeBuilder<Candlestick> builder)
    {
        builder
            .HasKey(candlestick => new
            {
                candlestick.Symbol,
                candlestick.Open,
                candlestick.High,
                candlestick.Low,
                candlestick.Close,
                candlestick.Volume,
                candlestick.Time,
            });

        builder
            .Property(candlestick => candlestick.Symbol)
            .HasMaxLength(Symbol.MaxLength)
            .HasConversion(
                symbol => symbol.Value,
                value => Symbol.From(value).ThrowIfFailure().Value)
            .IsRequired();
    }
}