using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder
            .HasKey(instrument => instrument.Id);

        builder
            .HasIndex(instrument => instrument.Symbol);

        builder
            .Property(instrument => instrument.Id)
            .HasMaxLength(InstrumentId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => InstrumentId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(instrument => instrument.Symbol)
            .HasMaxLength(Symbol.MaxLength)
            .HasConversion(
                symbol => symbol.Value,
                value => Symbol.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder
            .Property(instrument => instrument.Type)
            .HasEnumerationValueConversion()
            .IsRequired();

        builder
            .HasData([
                CreateInstrument(
                    symbol: "BINANCE:BTCUSDT.P",
                    description: "BTC / USDT Perpetual Swap",
                    type: InstrumentType.Swap),
                CreateInstrument(
                    symbol: "BINANCE:ETHUSDT.P",
                    description: "ETH / USDT Perpetual Swap",
                    type: InstrumentType.Swap),
                CreateInstrument(
                    symbol: "BINANCE:SOLUSDT.P",
                    description: "SOL / USDT Perpetual Swap",
                    type: InstrumentType.Swap),
            ]);
    }

    private static Instrument CreateInstrument(
        string symbol,
        string description,
        InstrumentType type) => Instrument
        .Create(
            InstrumentId.From(symbol).ThrowIfFailure().Value,
            Symbol.From(symbol).ThrowIfFailure().Value,
            description,
            type,
            DateTimeOffset.UtcNow)
        .ThrowIfError()
        .Value;
}
