namespace RichillCapital.Api.AcceptanceTests.Users;

public sealed class DeleteUserTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_DeleteUser()
    {
        var userIdToDelete = "1";

        var response = await Client.DeleteAsync($"/api/v1/users/{userIdToDelete}");
        response.EnsureSuccessStatusCode();
    }
}
