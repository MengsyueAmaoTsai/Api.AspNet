using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

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

public interface IInstrumentInitializationJob
{
    Task ProcessAsync();
}

internal sealed record SearchFundResponse
{
    public required SearchFundItemsResponse Items { get; init; }
    public required int StatusCode { get; init; }
    public required string Message { get; init; }
}

internal sealed record SearchFundItemsResponse
{
    [JsonProperty("current_page")]
    public required int CurrentPage { get; init; }

    [JsonProperty("first_page_url")]
    public required string FirstPageUrl { get; init; }

    [JsonProperty("last_page_url")]
    public required string LastPageUrl { get; init; }

    [JsonProperty("next_page_url")]
    public required string NextPageUrl { get; init; }

    [JsonProperty("prev_page_url")]
    public required string PrevPageUrl { get; init; }

    [JsonProperty("per_page")]
    public required int PerPage { get; init; }

    [JsonProperty("last_page")]
    public required int LastPage { get; init; }

    public required string Path { get; init; }
    public required int? From { get; init; }
    public required int? To { get; init; }
    public required int Total { get; init; }
    public required IEnumerable<FundResponse> Data { get; init; }
}

internal sealed record FundResponse
{
    // Meta
    public required string CnyesId { get; init; }
    public required string FundYesId { get; init; }
    public required string DisplayNameLocal { get; init; } // e.g. 景順全歐洲企業基金A股 歐元
    public required int DisplayNameLocalWithKwd { get; init; }
    public required string CategoryAbbr { get; init; } // e.g. 股票-歐洲
    public required string ClassCurrencyLocal { get; init; } // e.g. 歐元
    public required long InceptionDate { get; init; }
    public required string InvestmentArea { get; init; }

    [JsonProperty("portfolio_errors")]
    public required string PortfolioErrors { get; init; }

    [JsonProperty("lastUpdate")]
    public required long LastUpdated { get; init; }

    // Quotations
    public required decimal Nav { get; init; }
    public required decimal Change { get; init; }
    public required decimal ChangePercent { get; init; }
    public required decimal PrevPrice { get; init; }
    public required long PriceDate { get; init; }
    public required decimal? Return1Month { get; init; }

    // Cnyes settings
    public required bool ForSale { get; init; }
    public required string ForSaleUrl { get; init; }
    public required IEnumerable<FundPromoteResponse> FundTags { get; init; }
}

internal sealed record FundPromoteResponse
{
    public required string PromoteName { get; init; }
    public required string PromoteDescription { get; init; }
    public required long StartAt { get; init; }
    public required long EndAt { get; init; }
}