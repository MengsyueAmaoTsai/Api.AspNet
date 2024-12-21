using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Brokerages.Binance;

public sealed class BinanceBrokerage(
    ILogger<BinanceBrokerage> _logger,
    BinanceUsdMRestService _usdMRestService)
{
    public async Task<Result<IEnumerable<Account>>> ListAccountsAsync(
        CancellationToken cancellationToken = default)
    {
        return Result<IEnumerable<Account>>.With([]);
    }
}