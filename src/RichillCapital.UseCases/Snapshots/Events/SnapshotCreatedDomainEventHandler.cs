using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Snapshots.Events;

internal sealed class SnapshotCreatedDomainEventHandler(
    ILogger<SnapshotCreatedDomainEventHandler> _logger,
    IReadOnlyRepository<SignalSource> _signalSourceRepository) :
    IDomainEventHandler<SnapshotCreatedDomainEvent>
{
    public async Task Handle(
        SnapshotCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Snapshot with ID '{SnapshotId}' was created.",
            domainEvent.SnapshotId);

        var maybeSource = await _signalSourceRepository
            .FirstOrDefaultAsync(
                source => source.Id == domainEvent.SignalSourceId,
                cancellationToken)
            .ThrowIfNull();

        var source = maybeSource.Value;

        if (source.Stage == SignalSourceStage.Development ||
            source.Stage == SignalSourceStage.BackTesting)
        {
            return;
        }

        var content = new StringBuilder()
            .AppendLine($"[{domainEvent.GetType().Name}]")
            .AppendLine($"Time: {domainEvent.Time:yyyy-MM-dd HH:mm:ss.fff} - [{domainEvent.SignalSourceId}] Latency: {domainEvent.Latency}ms")
            .AppendLine($"Message: {(string.IsNullOrEmpty(domainEvent.Message) ? "No message" : domainEvent.Message)}")
            .ToString();

        await SendToTelegramDefaultAsync(content);
    }

    private async Task SendToTelegramDefaultAsync(string message)
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
