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

        builder
            .Property(source => source.Stage)
            .HasEnumerationValueConversion()
            .IsRequired();

        builder.HasData([
            CreateSignalSource(
                id: "TV-DEMO-LONG",
                name: "TV-DEMO-LONG",
                description: "TradingView Demo Long Signal Source",
                version: "1.0.0",
                stage: SignalSourceStage.Development,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero)),

            CreateSignalSource(
                id: "TV-DEMO-SHORT",
                name: "TV-DEMO-SHORT",
                description: "TradingView Demo Short Signal Source",
                version: "1.0.0",
                stage: SignalSourceStage.Development,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 1, TimeSpan.Zero)),
            CreateSignalSource(
                id: "CT-DEMO-LONG",
                name: "CT-DEMO-LONG",
                description: "CTrader Demo Long Signal Source",
                version: "1.0.0",
                stage: SignalSourceStage.Development,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 2, TimeSpan.Zero)),

            CreateSignalSource(
                id: "CT-DEMO-SHORT",
                name: "CT-DEMO-SHORT",
                description: "CTrader Demo Short Signal Source",
                version: "1.0.0",
                stage: SignalSourceStage.Development,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 3, TimeSpan.Zero)),

            CreateSignalSource(
                id: "TV-BINANCE:BTCUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 4, TimeSpan.Zero)),
            CreateSignalSource(
                id: "TV-BINANCE:BTCUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 5, TimeSpan.Zero)),

            CreateSignalSource(
                id: "TV-BINANCE:ETHUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 6, TimeSpan.Zero)),
            CreateSignalSource(
                id: "TV-BINANCE:ETHUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 7, TimeSpan.Zero)),

            CreateSignalSource(
                id: "TV-BINANCE:SOLUSDT.P-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 8, TimeSpan.Zero)),
            CreateSignalSource(
                id: "TV-BINANCE:SOLUSDT.P-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 9, TimeSpan.Zero)),

            CreateSignalSource(
                id: "CT-PEPPERSTONE:NAS100-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 10, TimeSpan.Zero)),
            CreateSignalSource(
                id: "CT-PEPPERSTONE:NAS100-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 11, TimeSpan.Zero)),

            CreateSignalSource(
                id: "CT-PEPPERSTONE:GER40-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 12, TimeSpan.Zero)),
            CreateSignalSource(
                id: "CT-PEPPERSTONE:GER40-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 13, TimeSpan.Zero)),

            CreateSignalSource(
                id: "CT-PEPPERSTONE:HK50-M15L-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 14, TimeSpan.Zero)),
            CreateSignalSource(
                id: "CT-PEPPERSTONE:HK50-M15S-001",
                name: "",
                description: "",
                version: "1.0.0",
                stage: SignalSourceStage.Simulation,
                createdTime: new DateTimeOffset(2021, 1, 1, 0, 0, 15, TimeSpan.Zero)),
        ]);
    }

    private static SignalSource CreateSignalSource(
        string id,
        string name,
        string description,
        string version,
        SignalSourceStage stage,
        DateTimeOffset createdTime = default) => SignalSource
        .Create(
            SignalSourceId.From(id).ThrowIfFailure().Value,
            name,
            description,
            version,
            stage,
            createdTime == default ? DateTimeOffset.UtcNow : createdTime)
        .ThrowIfError()
        .Value;
}