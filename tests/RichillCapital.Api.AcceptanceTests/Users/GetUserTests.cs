using System.Net.Http.Json;

using RichillCapital.Api.Contracts.Users;

using FluentAssertions;

namespace RichillCapital.Api.AcceptanceTests.Users;

public sealed class GetUserTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ReturnUser()
    {
        var expectedUserId = "1";

        var user = await Client.GetFromJsonAsync<UserDetailsResponse>($"/api/v1/users/{expectedUserId}");

        user.Should().NotBeNull();
        user!.Id.Should().Be(expectedUserId);
    }
}
