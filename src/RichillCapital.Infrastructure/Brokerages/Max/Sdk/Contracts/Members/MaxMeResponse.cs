namespace RichillCapital.Infrastructure.Brokerages.Max.Sdk.Contracts.Members;

public sealed record MaxMeResponse
{
    public required IEnumerable<MaxAccountResponse> Accounts { get; init; }
}

public sealed record MaxAccountResponse
{
    public required string Currency { get; init; }
    public required decimal Balance { get; init; }
    public required decimal Locked { get; init; }
    // public required decimal Staked { get; init; }
    public required string Type { get; init; }
    public required string FiatCurrency { get; init; }
    public required decimal FiatBalance { get; init; }
}