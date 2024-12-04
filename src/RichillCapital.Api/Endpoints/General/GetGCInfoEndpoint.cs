using RichillCapital.Api.Contracts.General;

namespace RichillCapital.Api.Endpoints.General;

internal static class GetGCInfoEndpoint
{
    internal static void MapGCInfoEndpoint(
        this IEndpointRouteBuilder builder,
        string path = "/api/v1/gc-info")
    {
        builder.MapGet(path, () =>
        {
            var gcMemoryInfo = GC.GetGCMemoryInfo();

            return Results.Ok(new GCInfoResponse
            {
                Time = DateTimeOffset.UtcNow,
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                TotalMemory = GC.GetTotalMemory(false),
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                TotalPauseDuration = GC.GetTotalPauseDuration(),

                GCMemoryInfo = new GCMemoryInfoResponse
                {
                    HighMemoryLoadThresholdBytes = gcMemoryInfo.HighMemoryLoadThresholdBytes,
                    MemoryLoadBytes = gcMemoryInfo.MemoryLoadBytes,
                    HeapSizeBytes = gcMemoryInfo.HeapSizeBytes,
                    FragmentedBytes = gcMemoryInfo.FragmentedBytes,
                    PauseTimePercentage = gcMemoryInfo.PauseTimePercentage,
                    PinnedObjectsCount = gcMemoryInfo.PinnedObjectsCount,
                },
            });
        })
            .WithTags(ApiTags.General)
            .AllowAnonymous();
    }
}