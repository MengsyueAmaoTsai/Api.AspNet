using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.BackgroundJobs;

internal static class ApiRoutes
{
    internal const string SearchFunds = "/fund/api/v2/search/fund";
}

internal sealed record SearchFundsRequest
{
    internal static readonly string[] DefaultFields = [
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
}
internal sealed class AnueClient(
    ILogger<AnueClient> _logger,
    HttpClient _httpClient)
{
    internal async Task<Result<SearchFundResponse>> SearchFundsAsync(
        int page = 1,
        string sortBy = "priceDate",
        string orderBy = "desc",
        int institutional = 0,
        int isShowTag = 1,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "fields", string.Join(",", SearchFundsRequest.DefaultFields) },
            { "page", page },
            { "institutional", institutional },
            { "isShowTag", isShowTag },
            { "order", sortBy },
            { "sort", orderBy },
        };

        var queryString = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiRoutes.SearchFunds}?{queryString}");

        _logger.LogInformation("Searching for page {page}", page);

        var httpResponseResult = await InvokeRequestAsync(httpRequest, cancellationToken);

        if (httpResponseResult.IsFailure)
        {
            return Result<SearchFundResponse>.Failure(httpResponseResult.Error);
        }

        var httpResponse = httpResponseResult.Value;

        return await HandleResponseAsync<SearchFundResponse>(httpResponse, cancellationToken);
    }

    private async Task<Result<HttpResponseMessage>> InvokeRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return Result<HttpResponseMessage>.With(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to invoke request: {error}", ex.Message);
            return Result<HttpResponseMessage>.Failure(Error.Unexpected(ex.Message));
        }
    }

    private async Task<Result<TResponse>> HandleResponseAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            return await HandleFailureAsync<TResponse>(httpResponse, cancellationToken);
        }

        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        var response = JsonConvert.DeserializeObject<TResponse>(content);

        return Result<TResponse>.With(response);
    }

    private async Task<Result<TResponse>> HandleFailureAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogError("Failed to search fund: {content}", content);
        return Result<TResponse>.Failure(Error.Unexpected(content));
    }
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