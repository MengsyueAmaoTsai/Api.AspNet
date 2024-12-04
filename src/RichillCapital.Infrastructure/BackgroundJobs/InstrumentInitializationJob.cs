using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.BackgroundJobs;

internal sealed class InstrumentInitializationJob(
    ILogger<InstrumentInitializationJob> _logger,
    HttpClient _httpClient,
    IDateTimeProvider _dateTimeProvider,
    IRepository<Instrument> _instrumentRepository,
    IUnitOfWork _unitOfWork) :
    IInstrumentInitializationJob
{
    private const string SearchPath = "/fund/api/v2/search/fund";

    public async Task ProcessAsync()
    {
        var firstPageResult = await SearchFundAsync(page: 1);

        if (firstPageResult.IsFailure)
        {
            _logger.LogError("Failed to search fund. {error}", firstPageResult.Error);
            return;
        }

        var totalPage = firstPageResult.Value.Items.LastPage;
        _logger.LogInformation("Total pages: {totalPage}", totalPage);

        for (var page = 1; page <= totalPage; page++)
        {
            var searchResult = await SearchFundAsync(page);

            if (searchResult.IsFailure)
            {
                _logger.LogError("Failed to search fund. {error}", searchResult.Error);
                continue;
            }

            await ProcessAndSaveFundsAsync(searchResult.Value.Items.Data);
        }

        _logger.LogInformation("Finished processing funds");
    }

    private async Task ProcessAndSaveFundsAsync(IEnumerable<FundResponse> funds)
    {
        var errorOrInstruments = new List<ErrorOr<Instrument>>();

        foreach (var fund in funds)
        {
            var errorOr = Instrument
                .Create(InstrumentId.From(fund.CnyesId).ThrowIfFailure().Value,
                Symbol.From(fund.CnyesId).ThrowIfFailure().Value,
                fund.DisplayNameLocal,
                InstrumentType.Index,
                _dateTimeProvider.UtcNow);

            if (errorOr.HasError)
            {
                _logger.LogWarning("Failed to create instrument. {error}", errorOr.Errors.First());
            }

            errorOrInstruments.Add(errorOr);
        }

        var validatedInstruments = errorOrInstruments
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

    private async Task<Result<SearchFundResponse>> SearchFundAsync(
        int page = 1,
        string sortBy = "priceDate",
        string orderBy = "desc",
        int institutional = 0,
        int isShowTag = 1)
    {
        string[] defaultFields = [
            "categoryAbbr",
            "change",
            "changePercent",
            "classCurrencyLocal",
            "cnyesId",
            "displayNameLocal",
            "displayNameLocalWithKwd",
            "forSale",
            "forSale",
            "inceptionDate",
            "investmentArea",
            "lastUpdate",
            "nav",
            "prevPrice",
            "priceDate",
            "return1Month",
            "saleStatus",
        ];

        var parameters = new Dictionary<string, object>
        {
            { "fields", string.Join(",", defaultFields) },
            { "page", page },
            { "institutional", institutional },
            { "isShowTag", isShowTag },
            { "order", sortBy },
            { "sort", orderBy },
        };

        var queryString = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"{SearchPath}?{queryString}");

        _logger.LogInformation("Searching for page {page}", page);

        var httpResponse = await _httpClient.SendAsync(httpRequest);
        var content = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to search fund: {content}", content);
            return Result<SearchFundResponse>.Failure(Error.Unexpected(content));
        }

        var response = JsonConvert.DeserializeObject<SearchFundResponse>(content);

        return Result<SearchFundResponse>.With(response);
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
    public required int From { get; init; }
    public required int To { get; init; }
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