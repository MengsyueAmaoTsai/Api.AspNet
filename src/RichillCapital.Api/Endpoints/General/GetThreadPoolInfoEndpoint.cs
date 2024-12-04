using RichillCapital.Api.Contracts.General;

namespace RichillCapital.Api.Endpoints.General;

internal static class GetThreadPoolInfoEndpoint
{
    internal static void MapThreadPoolInfoEndpoint(
        this IEndpointRouteBuilder builder,
        string path = "/api/v1/thread-pool-info")
    {
        builder
            .MapGet(path, () =>
            {
                ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableCompletionPortThreads);
                ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
                ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);

                return Results.Ok(new ThreadPoolInfoResponse
                {
                    Time = DateTimeOffset.Now,
                    MachineName = Environment.MachineName,
                    ProcessId = Environment.ProcessId,
                    ThreadCount = ThreadPool.ThreadCount,
                    CompletedWorkItemCount = ThreadPool.CompletedWorkItemCount,
                    PendingWorkItemCount = ThreadPool.PendingWorkItemCount,
                    AvailableWorkerThreads = availableWorkerThreads,
                    AvailableCompletionPortThreads = availableCompletionPortThreads,
                    MinWorkerThreads = minWorkerThreads,
                    MinCompletionPortThreads = minCompletionPortThreads,
                    MaxWorkerThreads = maxWorkerThreads,
                    MaxCompletionPortThreads = maxCompletionPortThreads,
                });
            })
            .WithTags(ApiTags.General)
            .AllowAnonymous();
    }
}