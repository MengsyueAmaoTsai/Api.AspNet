using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Snapshots;

[ApiVersion(EndpointVersion.V1)]
public sealed class GetSnapshotStatisticsEndpoint(
    IReadOnlyRepository<Snapshot> _repository) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult
{
    [HttpGet(ApiRoutes.Snapshots.Statistics)]
    [SwaggerOperation(Tags = [ApiTags.Snapshots])]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var snapshots = await _repository.ListAsync(cancellationToken);

        if (snapshots.Count == 0)
        {
            return NoContent();
        }

        var latencies = snapshots
            .Select(x => x.Latency)
            .ToList();

        var mean = latencies.Average(l => l.Value);
        var max = latencies.Max(l => l.Value);
        var min = latencies.Min(l => l.Value);
        var std = CalculateStandardDeviation(latencies, mean);

        var latencyCounts = latencies
            .Select(ClassifyLatency)
            .GroupBy(category => category)
            .ToDictionary(group => group.Key, group => group.Count());

        return Ok(new
        {
            Count = latencies.Count,
            LatencyStatistics = new
            {
                Mean = mean,
                Max = max,
                Min = min,
                StandardDeviation = std,
                normals = latencyCounts.GetValueOrDefault("Normal", 0),
                normalPercentage = Math.Round(
                    (double)latencyCounts.GetValueOrDefault("Normal", 0) / latencies.Count * 100, 2),
                normalMean = Math.Round(
                    latencies.Where(l => ClassifyLatency(l) == "Normal").Average(l => l.Value), 2),
                minors = latencyCounts.GetValueOrDefault("Minor", 0),
                minorPercentage = Math.Round(
                    (double)latencyCounts.GetValueOrDefault("Minor", 0) / latencies.Count * 100, 2),
                minorMean = Math.Round(
                    latencies.Where(l => ClassifyLatency(l) == "Minor").Average(l => l.Value), 2),
                moderates = latencyCounts.GetValueOrDefault("Moderate", 0),
                moderatePercentage = Math.Round(
                    (double)latencyCounts.GetValueOrDefault("Moderate", 0) / latencies.Count * 100, 2),
                moderateMean = Math.Round(
                    latencies.Where(l => ClassifyLatency(l) == "Moderate").Average(l => l.Value), 2),
                severes = latencyCounts.GetValueOrDefault("Severe", 0),
                severePercentage = Math.Round(
                    (double)latencyCounts.GetValueOrDefault("Severe", 0) / latencies.Count * 100, 2),
                severeMean = Math.Round(
                    latencies.Where(l => ClassifyLatency(l) == "Severe").Average(l => l.Value), 2)
            }
        });
    }

    private double CalculateStandardDeviation(
        List<Latency> latencies,
        double mean)
    {
        var values = latencies.Select(l => l.Value).ToList();

        var sumOfSquares = values
            .Select(v => Math.Pow(v - mean, 2))
            .Sum();

        return Math.Sqrt(sumOfSquares / values.Count);
    }

    private string ClassifyLatency(Latency latency)
    {
        var defaultMean = 2500m;
        var defaultStandardDeviation = 1500m;

        if (latency.Value >= (defaultMean - defaultStandardDeviation)
            && latency.Value <= (defaultMean + defaultStandardDeviation))
        {
            return "Normal";
        }

        if (latency.Value >= (defaultMean - 2 * defaultStandardDeviation)
            && latency.Value <= (defaultMean + 2 * defaultStandardDeviation))
        {
            return "Minor";
        }

        if (latency.Value >= (defaultMean - 3 * defaultStandardDeviation)
            && latency.Value <= (defaultMean + 3 * defaultStandardDeviation))
        {
            return "Moderate";
        }

        return "Severe";
    }
}