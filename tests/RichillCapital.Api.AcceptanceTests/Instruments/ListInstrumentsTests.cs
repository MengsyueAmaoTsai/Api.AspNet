using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts;

namespace RichillCapital.Api.AcceptanceTests.Instruments;

public sealed class ListInstrumentsTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ListUsers()
    {
        var instruments = await Client.GetFromJsonAsync<IEnumerable<InstrumentResponse>>("/api/v1/instruments");

        instruments.Should().NotBeNullOrEmpty();
    }
}
