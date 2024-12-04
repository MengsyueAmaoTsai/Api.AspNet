using System.Net.Http.Json;

using RichillCapital.Api.Contracts.Users;

using FluentAssertions;

namespace RichillCapital.Api.AcceptanceTests.Users;

public sealed class CreateUserTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_CreateUser()
    {
        var request = new CreateUserRequest
        {
            Email = "new@example.com",
            Name = "Test user",
            Password = "password",
        };

        var response = await Client.PostAsJsonAsync("/api/v1/users", request);

        response.EnsureSuccessStatusCode();

        var createdResponse = await response.Content.ReadFromJsonAsync<UserCreatedResponse>();

        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().NotBeEmpty();

        var user = await Client.GetFromJsonAsync<UserResponse>($"/api/v1/users/{createdResponse.Id}");

        user.Should().NotBeNull();
        user!.Id.Should().Be(createdResponse.Id);
        user!.Email.Should().Be(request.Email);
        user!.Name.Should().Be(request.Name);
    }
}
