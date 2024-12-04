using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts;

namespace RichillCapital.Api.AcceptanceTests.Instruments;

public sealed class GetInstrumentTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ReturnUser()
    {
        var expectedSymbol = "TPEX";

        var instrument = await Client.GetFromJsonAsync<InstrumentDetailsResponse>($"/api/v1/instruments/{expectedSymbol}");

        instrument.Should().NotBeNull();
        instrument!.Symbol.Should().Be(expectedSymbol);
    }
}
