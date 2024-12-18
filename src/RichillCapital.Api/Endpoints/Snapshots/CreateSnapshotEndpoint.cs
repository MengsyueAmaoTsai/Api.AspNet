using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Snapshots;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Snapshots.Commands;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Snapshots;

[ApiVersion(EndpointVersion.V1)]
public sealed class CreateSnapshotEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<CreateSnapshotRequest>
    .WithActionResult<SnapshotCreatedResponse>
{
    [HttpPost(ApiRoutes.Snapshots.Create)]
    [SwaggerOperation(Tags = [ApiTags.Snapshots])]
    [AllowAnonymous]
    public override async Task<ActionResult<SnapshotCreatedResponse>> HandleAsync(
        [FromBody] CreateSnapshotRequest request,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<CreateSnapshotRequest>
            .With(request)
            .Then(req => new CreateSnapshotCommand
            {
                SignalSourceId = req.SignalSourceId,
                Time = req.Time,
                Symbol = req.Symbol,
                BarTime = req.BarTime,
                LastPrice = req.LastPrice,
                Message = req.Message,
            })
            .Then(command => _mediator.Send(command, cancellationToken))
            .Then(id => new SnapshotCreatedResponse { Id = id.Value })
            .Match(HandleFailure, Ok);
}
