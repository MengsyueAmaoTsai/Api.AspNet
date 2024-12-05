using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts.SignalSources;

namespace RichillCapital.Api.AcceptanceTests.SignalSources;

public sealed class CreateSignalSourceTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_CreateSignalSource()
    {
        var request = new CreateSignalSourceRequest
        {
            Id = "NewId",
            Name = "NewName",
            Description = "NewDescription",
            Version = "NewVersion",
            Stage = "Development",
        };

        var response = await Client.PostAsJsonAsync("/api/v1/signal-sources", request);
        response.EnsureSuccessStatusCode();

        var createdResponse = await response.Content.ReadFromJsonAsync<SignalSourceCreatedResponse>();
        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().Be(request.Id);

        var source = await Client.GetFromJsonAsync<SignalSourceDetailsResponse>($"/api/v1/signal-sources/{request.Id}");

        source.Should().NotBeNull();
        source!.Id.Should().Be(request.Id);
        source!.Name.Should().Be(request.Name);
        source!.Description.Should().Be(request.Description);
        source!.Version.Should().Be(request.Version);
    }
}