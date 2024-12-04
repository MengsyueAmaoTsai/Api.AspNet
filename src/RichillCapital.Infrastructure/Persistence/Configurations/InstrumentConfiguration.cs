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
                    symbol: "TWSE",
                    description: "Taiwan Stock Exchange Index",
                    type: InstrumentType.Index),
                CreateInstrument(
                    symbol: "TPEX",
                    description: "Taipei Security Exchange Index",
                    type: InstrumentType.Index),
                CreateInstrument(
                    symbol: "BTCUSDT.P",
                    description: "Bitcoin to US Dollar Perpetual Swap",
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
