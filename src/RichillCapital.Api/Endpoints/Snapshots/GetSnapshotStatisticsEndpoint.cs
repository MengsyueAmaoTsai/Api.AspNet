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

        var mean = latencies.Average();
        var max = latencies.Max();
        var min = latencies.Min();
        var std = CalculateStandardDeviation(latencies, mean);

        var latencyCounts = latencies
            .Select(latency => ClassifyLatency(latency, mean, std))
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
                minors = latencyCounts.GetValueOrDefault("Minor", 0),
                moderates = latencyCounts.GetValueOrDefault("Moderate", 0),
                severes = latencyCounts.GetValueOrDefault("Severe", 0)
            }
        });
    }

    private double CalculateStandardDeviation(
        List<int> values,
        double mean)
    {
        var sumOfSquares = values
            .Select(v => Math.Pow(v - mean, 2))
            .Sum();

        return Math.Sqrt(sumOfSquares / values.Count);
    }

    private string ClassifyLatency(double latency, double mean, double std)
    {
        if (latency >= (mean - std) && latency <= (mean + std))
        {
            return "Normal";
        }
        else if (latency >= (mean - 2 * std) && latency <= (mean + 2 * std))
        {
            return "Minor";
        }
        else if (latency >= (mean - 3 * std) && latency <= (mean + 3 * std))
        {
            return "Moderate";
        }
        else
        {
            return "Severe";
        }
    }
}