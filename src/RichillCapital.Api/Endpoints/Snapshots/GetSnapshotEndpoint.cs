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
public sealed class GetSnapshotEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<string>
    .WithActionResult<SnapshotDetailsResponse>
{
    [HttpGet(ApiRoutes.Snapshots.Get)]
    [SwaggerOperation(Tags = [ApiTags.Snapshots])]
    [AllowAnonymous]
    public override async Task<ActionResult<SnapshotDetailsResponse>> HandleAsync(
        [FromRoute(Name = nameof(snapshotId))] string snapshotId,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<string>
            .With(snapshotId)
            .Then(id => new GetSnapshotQuery { SnapshotId = id })
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(dto => dto.ToDetailsResponse())
            .Match(HandleFailure, Ok);
}