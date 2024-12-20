using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Events;

internal sealed class SnapshotCreatedDomainEventHandler(
    ILogger<SnapshotCreatedDomainEventHandler> _logger) :
    IDomainEventHandler<SnapshotCreatedDomainEvent>
{
    public async Task Handle(
        SnapshotCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Snapshot with ID '{SnapshotId}' was created.",
            domainEvent.SnapshotId);

        await NotificationAsync(domainEvent);
    }

    private async Task NotificationAsync(SnapshotCreatedDomainEvent domainEvent)
    {
        var content = new StringBuilder()
            .AppendLine($"Time: {domainEvent.Time:yyyy-MM-dd HH:mm:ss.fff}")
            .AppendLine($"Source: {domainEvent.SignalSourceId}")
            .AppendLine("--------------- Data set ---------------")
            .AppendLine($"Symbol: {domainEvent.Symbol}")
            .AppendLine($"Bar time: {domainEvent.BarTime:yyyy-MM-dd HH:mm:ss.fff}")
            .AppendLine($"Last price: {domainEvent.LastPrice}")
            .AppendLine("--------------- Order ---------------")
            .AppendLine("--------------- Position ---------------")
            .AppendLine("--------------- Additional info ---------------")
            .AppendLine($"Latency: {domainEvent.Latency} ms")
            .AppendLine(string.IsNullOrEmpty(domainEvent.Message) ? string.Empty : domainEvent.Message)
            .ToString();

        await SendToSlackAsync(content);
        await SendToDiscordWebhookAsync(content);
        await SendToTelegramAsync(content);
    }

    private async Task SendToTelegramAsync(string message)
    {
        var botToken = "7528339438:AAHhzsG26Ki-TefkWZj-WnJn-Wdh1vynqv0";
        var chatId = "-1002280797874";

        var telegramMessage = new
        {
            chat_id = chatId,
            text = message
        };

        var telegramMessageJson = JsonSerializer.Serialize(telegramMessage);

        var telegramMessageContent = new StringContent(telegramMessageJson, Encoding.UTF8, "application/json");

        var telegramApiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";

        using var httpClient = new HttpClient();

        var response = await httpClient.PostAsync(telegramApiUrl, telegramMessageContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send telegram message. Status code: {StatusCode}. Reason: {ReasonPhrase}.",
                response.StatusCode,
                response.ReasonPhrase);
        }
    }

    private async Task SendToSlackAsync(string message)
    {
        var slackWebhookUrl = "https://hooks.slack.com/services/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";

        var slackMessage = new
        {
            text = message,
        };

        var slackMessageJson = JsonSerializer.Serialize(slackMessage);

        var slackMessageContent = new StringContent(slackMessageJson, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();

        var response = await httpClient.PostAsync(slackWebhookUrl, slackMessageContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send slack webhook. Status code: {StatusCode}. Reason: {ReasonPhrase}.",
                response.StatusCode,
                response.ReasonPhrase);
        }
    }

    private async Task SendToDiscordWebhookAsync(string message)
    {
        var webhookUrl = "https://discord.com/api/webhooks/1250632910323843133/-TPL3vIFrBMgpzeYJRjZcV90vVwBBpls9RCdoP9Q7fbyiZmngM219CCUwetEbX7SrfdI";

        var discordMessage = new
        {
            content = message
        };

        var discordMessageJson = JsonSerializer.Serialize(discordMessage);

        var discordMessageContent = new StringContent(discordMessageJson, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();

        var response = await httpClient.PostAsync(webhookUrl, discordMessageContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send discord webhook. Status code: {StatusCode}. Reason: {ReasonPhrase}.",
                response.StatusCode,
                response.ReasonPhrase);
        }
    }
}