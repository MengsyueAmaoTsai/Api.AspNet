using System.Net.Http.Json;

using RichillCapital.Api.Contracts.Users;

using FluentAssertions;

namespace RichillCapital.Api.AcceptanceTests.Users;

public sealed class ListUsersTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ListUsers()
    {
        var users = await Client.GetFromJsonAsync<IEnumerable<UserResponse>>("/api/v1/users");

        users.Should().NotBeNullOrEmpty();
    }
}
