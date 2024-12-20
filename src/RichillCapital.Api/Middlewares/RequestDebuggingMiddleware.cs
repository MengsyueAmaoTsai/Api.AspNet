﻿using System.Diagnostics;
using System.Text;

namespace RichillCapital.Api.Middlewares;

internal sealed class RequestDebuggingMiddleware(
    ILogger<RequestDebuggingMiddleware> _logger) :
    IMiddleware
{
    private const string AnonymousUserName = "Anonymous";
    private const string NoHeaders = "No headers";
    private const string NoQueryString = "No query string";
    private const string NoRequestBody = "No request body";
    private const string NoResponseBody = "No response body";

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var currentUser = context.User.Identity?.Name ?? AnonymousUserName;
        var remoteIpAddress = context.Connection.RemoteIpAddress;
        var queryString = context.Request.QueryString;
        var requestBodyInfo = await ReadRequestBodyAsync(context.Request);

        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation(
            "Incoming request - {method} {path} from {user}@{remoteAddress}",
            method,
            path,
            currentUser,
            remoteIpAddress);

        var originalResponseBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        responseBody.Seek(0, SeekOrigin.Begin);

        var responseBodyInfo = await new StreamReader(responseBody).ReadToEndAsync();

        responseBody.Seek(0, SeekOrigin.Begin);

        await responseBody.CopyToAsync(originalResponseBodyStream);

        var elapsedMilliseconds = (int)stopwatch.Elapsed.TotalMilliseconds;
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation(
            "Outgoing response - {statusCode} for {method} {path}. Elapsed: {elapsed}ms",
            statusCode,
            method,
            path,
            elapsedMilliseconds);

        LogHttpContextDetailsIfError(
            context,
            queryString.ToString(),
            requestBodyInfo,
            responseBodyInfo);
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return body;
    }

    private void LogHttpContextDetailsIfError(
        HttpContext context,
        string queryString,
        string requestBodyInfo,
        string responseBodyInfo)
    {
        if (context.Response.StatusCode < 400)
        {
            return;
        }

        var requestHeaders = context.Request.Headers
            .Select(header => $"{header.Key}: {header.Value}").ToArray();

        var responseHeaders = context.Response.Headers
            .Select(header => $"{header.Key}: {header.Value}").ToArray();

        var requestHeaderInfo = requestHeaders.Any() ?
            string.Join("\n", requestHeaders) :
            NoHeaders;

        var responseHeaderInfo = requestHeaders.Any() ?
            string.Join("\n", responseHeaders) :
            NoHeaders;

        var logMessage = new StringBuilder()
            .AppendLine("----- Request Details -----")
            .AppendLine($"Request Headers:\n{requestHeaderInfo}")
            .AppendLine($"QueryString: {queryString ?? NoQueryString}")
            .AppendLine($"Request Body:\n{requestBodyInfo ?? NoRequestBody}")
            .AppendLine("----- Response Details -----")
            .AppendLine($"Response Headers:\n{responseHeaderInfo}")
            .AppendLine($"Response Body:\n{responseBodyInfo ?? NoResponseBody}")
            .ToString();

        _logger.LogWarning(logMessage);
    }
}
