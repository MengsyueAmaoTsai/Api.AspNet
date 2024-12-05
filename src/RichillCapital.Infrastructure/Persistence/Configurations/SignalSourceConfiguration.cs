using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Persistence.Configurations;

internal sealed class SignalSourceConfiguration : IEntityTypeConfiguration<SignalSource>
{
    public void Configure(EntityTypeBuilder<SignalSource> builder)
    {
        builder
            .HasKey(source => source.Id);

        builder
            .Property(source => source.Id)
            .HasMaxLength(SignalSourceId.MaxLength)
            .HasConversion(
                id => id.Value,
                value => SignalSourceId.From(value).ThrowIfFailure().Value)
            .IsRequired();

        builder.HasData([
            CreateSignalSource(
                id: "TV-BINANCE:BTCUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0"),
            CreateSignalSource(
                id: "TV-BINANCE:BTCUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0"),

            CreateSignalSource(
                id: "TV-BINANCE:ETHUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0"),
            CreateSignalSource(
                id: "TV-BINANCE:ETHUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0"),

            CreateSignalSource(
                id: "TV-BINANCE:SOLUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0"),
            CreateSignalSource(
                id: "TV-BINANCE:SOLUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0"),
        ]);
    }

    private static SignalSource CreateSignalSource(
        string id,
        string name,
        string description,
        string version) => SignalSource
        .Create(
            SignalSourceId.From(id).ThrowIfFailure().Value,
            name,
            description,
            version,
            DateTimeOffset.UtcNow)
        .ThrowIfError()
        .Value;
}