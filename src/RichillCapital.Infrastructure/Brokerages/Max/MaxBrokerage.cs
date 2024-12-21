using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
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

        var userInfo = userInfoResult.Value;

        _logger.LogError("User info: {info}", userInfo);

        var errorOrAccount = Account
            .Create(
                id: AccountId.From(MaxUid).ThrowIfFailure().Value,
                userId: UserId.From("UID0000001").ThrowIfFailure().Value,
                name: MaxUid,
                createdTime: DateTimeOffset.UtcNow);

        var account = errorOrAccount.ThrowIfError().Value;

        return Result<IEnumerable<Account>>.With([account]);
    }
}