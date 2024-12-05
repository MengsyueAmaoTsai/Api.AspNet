using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts.SignalSources;

namespace RichillCapital.Api.AcceptanceTests.SignalSources;

public sealed class GetSignalSourceTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ReturnSignalSource()
    {
        var expectedId = "TV-BINANCE:BTCUSDT.P-M15L-001";

        var source = await Client.GetFromJsonAsync<SignalSourceDetailsResponse>($"/api/v1/signal-sources/{expectedId}");

        source.Should().NotBeNull();
        source!.Id.Should().Be(expectedId);
    }
}
