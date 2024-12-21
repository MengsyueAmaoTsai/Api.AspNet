using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.Infrastructure.Brokerages.Max.Sdk;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Brokerages.Max;

public sealed class MaxBrokerage(
    ILogger<MaxBrokerage> _logger,
    MaxRestService _restService)
{
    private const string MaxUid = "8e31ae1b-de3a-4c67-aeb3-a38b07508750";

    public async Task<Result<IEnumerable<Account>>> ListAccountsAsync(
        CancellationToken cancellationToken = default)
    {
        var userInfoResult = await _restService.GetUserInfoAsync(cancellationToken);

        if (userInfoResult.IsFailure)
        {
            return Result<IEnumerable<Account>>.Failure(userInfoResult.Error);
        }

        var memberProfileResult = await _restService.GetMemberProfileAsync(cancellationToken);

        if (memberProfileResult.IsFailure)
        {
            return Result<IEnumerable<Account>>.Failure(memberProfileResult.Error);
        }

        var meResult = await _restService.GetMeAsync(cancellationToken);

        if (meResult.IsFailure)
        {
            return Result<IEnumerable<Account>>.Failure(meResult.Error);
        }

        var userInfo = userInfoResult.Value;
        var memberProfile = memberProfileResult.Value;
        var me = meResult.Value;

        foreach (var maxAccount in me.Accounts)
        {
            _logger.LogInformation(
                "{currency}: {balance}",
                maxAccount.Currency,
                maxAccount.Balance);
        }

        var maxUid = memberProfile.Uid;

        var errorOrAccount = Account
            .Create(
                id: AccountId.From(maxUid).ThrowIfFailure().Value,
                userId: UserId.From("UID0000001").ThrowIfFailure().Value,
                name: $"MAX-{memberProfile.Name}",
                createdTime: DateTimeOffset.UtcNow);

        var account = errorOrAccount.ThrowIfError().Value;

        return Result<IEnumerable<Account>>.With([account]);
    }
}