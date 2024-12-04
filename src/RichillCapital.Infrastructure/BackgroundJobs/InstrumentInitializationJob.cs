using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.BackgroundJobs;

internal sealed class InstrumentInitializationJob(
    ILogger<InstrumentInitializationJob> _logger,
    IDateTimeProvider _dateTimeProvider,
    AnueClient _anueClient,
    IRepository<Instrument> _instrumentRepository,
    IUnitOfWork _unitOfWork) :
    IInstrumentInitializationJob
{
    public async Task ProcessAsync()
    {
        var result = await InitializeInstrumentsFromAnue();

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to initialize instruments. {error}", result.Error);
            return;
        }

        _logger.LogInformation("Successfully initialized instruments");
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
                    InstrumentType.Index,
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
