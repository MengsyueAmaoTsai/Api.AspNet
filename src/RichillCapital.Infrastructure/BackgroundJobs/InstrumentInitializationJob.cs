using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.Infrastructure.Brokerages.Max;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.BackgroundJobs;

internal sealed class InstrumentInitializationJob(
    ILogger<InstrumentInitializationJob> _logger,
    MaxBrokerage _maxBrokerage,
    IRepository<Account> _accountRepository,
    IDateTimeProvider _dateTimeProvider,
    AnueClient _anueClient,
    IRepository<Instrument> _instrumentRepository,
    IUnitOfWork _unitOfWork) :
    IInstrumentInitializationJob
{
    public async Task ProcessAsync()
    {
        // var result = await InitializeInstrumentsFromAnue();
        var result = await InitializeInstrumentsAsync(default);

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to initialize instruments. {error}", result.Error);
            return;
        }

        _logger.LogInformation("Successfully initialized instruments");
    }

    private async Task<Result> InitializeInstrumentsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _maxBrokerage.ListAccountsAsync(cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        var accounts = result.Value;

        foreach (var account in accounts)
        {
            if (await _accountRepository.AnyAsync(a => a.Id == account.Id, cancellationToken))
            {
                _logger.LogInformation("Account {id} already exists", account.Id);
                continue;
            }

            _accountRepository.Add(account);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }

    private async Task<Result> InitializeInstrumentsFromAnue()
    {
        var firstPageResult = await _anueClient.SearchFundsAsync(page: 1);

        if (firstPageResult.IsFailure)
        {
            return Result.Failure(firstPageResult.Error);
        }

        for (var page = 1; page <= firstPageResult.Value.Items.LastPage; page++)
        {
            var searchResult = await _anueClient.SearchFundsAsync(page);

            if (searchResult.IsFailure)
            {
                _logger.LogWarning("Failed to search fund. {error}", searchResult.Error);
                continue;
            }

            await ProcessAndSaveFundsAsync(searchResult.Value.Items.Data);
        }

        return Result.Success;
    }

    private async Task ProcessAndSaveFundsAsync(IEnumerable<FundResponse> funds)
    {
        var validatedInstruments = funds
            .Select(f =>
            {
                var errorOr = Instrument
                    .Create(InstrumentId.From(f.CnyesId).ThrowIfFailure().Value,
                    Symbol.From(f.CnyesId).ThrowIfFailure().Value,
                    f.DisplayNameLocal,
                    InstrumentType.MutualFund,
                    _dateTimeProvider.UtcNow);

                if (errorOr.HasError)
                {
                    _logger.LogWarning("Failed to create instrument. {error}", errorOr.Errors.First());
                }

                return errorOr;
            })
            .Where(errorOr => errorOr.IsValue)
            .Select(errorOr => errorOr.Value)
            .ToList();

        if (!validatedInstruments.Any())
        {
            _logger.LogWarning("No instruments to save");
            return;
        }

        _instrumentRepository.AddRange(validatedInstruments);

        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Saved {count} instruments", validatedInstruments.Count);
    }
}
