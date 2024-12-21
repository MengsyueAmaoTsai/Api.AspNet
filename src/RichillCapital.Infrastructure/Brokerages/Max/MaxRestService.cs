using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Infrastructure.Brokerages.Max;

public sealed class MaxRestService(
    ILogger<MaxRestService> _logger,
    HttpClient _httpClient)
{
    public async Task<Result<MaxUserInfoResponse>> GetUserInfoAsync(
        CancellationToken cancellationToken = default)
    {
        var path = "/api/v2/members/me";
        var parameters = new Dictionary<string, object>();

        return await InvokeRequestAsync<MaxUserInfoResponse>(
            HttpMethod.Get,
            path,
            parameters,
            requiresAuthentication: true,
            cancellationToken);
    }

    private async Task<Result<TResponse>> InvokeRequestAsync<TResponse>(
        HttpMethod method,
        string path,
        Dictionary<string, object> parameters,
        bool requiresAuthentication,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Invoking request {method} {path}", method, path);

        if (requiresAuthentication)
        {
            parameters["nonce"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        var request = CreateRequest(method, path, parameters, requiresAuthentication);

        var httpResponse = await _httpClient.SendAsync(request, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var maxErrorResponse = JsonConvert.DeserializeObject<MaxErrorResponse>(errorContent);

            var error = Error.Unexpected($"Failed to get user info. {maxErrorResponse.Error.Code} {maxErrorResponse.Error.Message}");
            return Result<TResponse>.Failure(error);
        }

        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        var response = JsonConvert.DeserializeObject<TResponse>(content);

        return Result<TResponse>.With(response);
    }

    private HttpRequestMessage CreateRequest(
      HttpMethod method,
      string path,
      Dictionary<string, object> parameter,
      bool requiresAuthentication)
    {
        var request = new HttpRequestMessage(method, BuildRequestPath(path, parameter));

        if (requiresAuthentication)
        {
            Dictionary<string, object> parametersToSign = [];

            parametersToSign.Add("path", path);

            foreach (var p in parameter)
            {
                parametersToSign.Add(p.Key, p.Value);
            }

            var (encodedPayload, signature) = GenerateSignature(
                "NqfZFUWlIlvegz6aW4xjDOCpLQS9ExjD7DV4PFQy",
                parametersToSign);

            request.AttachAuthenticationHeaderValues(encodedPayload, signature);
        }

        return request;
    }

    private static string BuildRequestPath(
        string path,
        Dictionary<string, object> parameters)
    {
        var requestPath = path;

        if (parameters.Count > 0)
        {
            var queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
            requestPath = $"{path}?{queryString}";
        }

        return requestPath;
    }

    internal (string, string) GenerateSignature(
        string secretKey,
        Dictionary<string, object> parameters)
    {
        var encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameters)));
        var signature = Sign(secretKey, encodedPayload);

        return (encodedPayload, signature);
    }

    internal string Sign(
        string secretKey,
        string payload)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

        return BitConverter
            .ToString(hash)
            .Replace("-", "")
            .ToLower();
    }
}

public sealed record MaxErrorResponse
{
    public required MaxErrorInternalResponse Error { get; init; }
}

public sealed record MaxErrorInternalResponse
{
    public required int Code { get; init; }
    public required string Message { get; init; }
}

public sealed record MaxUserInfoResponse
{
    public required string Email { get; init; }
    public required int Level { get; init; }
    public required bool MWalletEnabled { get; init; }
}

internal static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage AttachAuthenticationHeaderValues(
        this HttpRequestMessage request,
        string payload,
        string signature)
    {
        request.Headers.Add("X-MAX-PAYLOAD", payload);
        request.Headers.Add("X-MAX-SIGNATURE", signature);

        return request;
    }
}