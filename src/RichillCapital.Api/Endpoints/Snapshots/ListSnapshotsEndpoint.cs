using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Snapshots;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Snapshots.Queries;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Snapshots;

[ApiVersion(EndpointVersion.V1)]
public sealed class ListSnapshotsEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult<IEnumerable<SnapshotResponse>>
{
    [HttpGet(ApiRoutes.Snapshots.List)]
    [SwaggerOperation(Tags = [ApiTags.Snapshots])]
    [AllowAnonymous]
    public override async Task<ActionResult<IEnumerable<SnapshotResponse>>> HandleAsync(
        CancellationToken cancellationToken = default) =>
        await ErrorOr<ListSnapshotsQuery>
            .With(new ListSnapshotsQuery())
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(snapshots => snapshots
                .Select(s => s.ToResponse())
                .ToList())
            .Match(HandleFailure, Ok);
}