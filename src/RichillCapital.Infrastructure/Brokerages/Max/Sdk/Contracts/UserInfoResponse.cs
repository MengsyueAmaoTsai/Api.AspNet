namespace RichillCapital.Infrastructure.Brokerages.Max.Sdk.Contracts;

public sealed record MaxUserInfoResponse
{
    public required string Email { get; init; }
    public required int Level { get; init; }
    public required bool MWalletEnabled { get; init; }
}