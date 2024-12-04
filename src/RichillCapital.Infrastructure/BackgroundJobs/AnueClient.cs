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