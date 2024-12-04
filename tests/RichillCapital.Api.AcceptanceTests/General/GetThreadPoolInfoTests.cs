using System.Net;
using System.Net.Http.Json;

using RichillCapital.Api.Contracts.General;

using FluentAssertions;

namespace RichillCapital.Api.AcceptanceTests.General;

public sealed class GetThreadPoolInfoTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ReturnThreadPoolInfo()
    {
        var response = await Client.GetAsync("/api/v1/thread-pool-info");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var info = await response.Content.ReadFromJsonAsync<ThreadPoolInfoResponse>();

        info.Should().NotBeNull();
        info?.MachineName.Should().NotBeNullOrEmpty();
    }
}