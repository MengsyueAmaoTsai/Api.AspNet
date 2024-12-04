using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Instruments;
using RichillCapital.Domain;

namespace RichillCapital.Api.AcceptanceTests.Instruments;


public sealed class CreateInstrumentTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_CreateUser()
    {
        var request = new CreateInstrumentRequest
        {
            Symbol = "AAPL",
            Description = "Apple Inc.",
            Type = InstrumentType.Index.Name,
        };

        var response = await Client.PostAsJsonAsync("/api/v1/instruments", request);

        response.EnsureSuccessStatusCode();

        var createdResponse = await response.Content.ReadFromJsonAsync<InstrumentCreatedResponse>();

        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().NotBeEmpty();

        // var instrument = await Client.GetFromJsonAsync<InstrumentDetailsResponse>($"/api/v1/users/{createdResponse.Id}");

        // instrument.Should().NotBeNull();
        // instrument!.Id.Should().Be(createdResponse.Id);
        // instrument!.Symbol.Should().Be(request.Symbol);
        // instrument!.Description.Should().Be(request.Description);
        // instrument!.Type.Should().Be(request.Type);
    }
}