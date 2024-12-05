using System.Net.Http.Json;

using FluentAssertions;

using RichillCapital.Api.Contracts.Accounts;

namespace RichillCapital.Api.AcceptanceTests.Accounts;

public sealed class ListAccountsTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ListAccounts()
    {
        var accounts = await Client.GetFromJsonAsync<IEnumerable<AccountResponse>>("/api/v1/accounts");

        accounts.Should().NotBeNullOrEmpty();
    }
}

public sealed class GetAccountTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_ReturnAccount()
    {
        var accountId = "1";

        var account = await Client.GetFromJsonAsync<AccountDetailsResponse>($"/api/v1/accounts/{accountId}");

        account.Should().NotBeNull();
        account!.Id.Should().Be(accountId);
    }
}

public sealed class CreateAccountTests(
    EndToEndTestWebApplicationFactory factory) :
    AcceptanceTest(factory)
{
    [Fact]
    public async Task Should_CreateAccount()
    {
        var createAccountRequest = new CreateAccountRequest
        {
            UserId = "UID0000001",
            Name = "Test Account",
        };

        var response = await Client.PostAsJsonAsync("/api/v1/accounts", createAccountRequest);

        response.EnsureSuccessStatusCode();

        var createdResponse = await response.Content.ReadFromJsonAsync<AccountCreatedResponse>();

        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().NotBeNullOrEmpty();
    }
}