using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.BackgroundJobs;

internal sealed class AnueClient(
    ILogger<AnueClient> _logger,
    HttpClient _httpClient)
{
    private const string SearchPath = "/fund/api/v2/search/fund";

    internal async Task<Result<SearchFundResponse>> SearchFundAsync(
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