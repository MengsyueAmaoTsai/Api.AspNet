using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.SignalSubscriptions;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.SignalSubscriptions.Commands;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.SignalSubscriptions;

[ApiVersion(EndpointVersion.V1)]
public sealed class CreateSignalSubscriptionEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<CreateSignalSubscriptionRequest>
    .WithActionResult<SignalSubscriptionCreatedResponse>
{
    [HttpPost(ApiRoutes.SignalSubscriptions.Create)]
    [SwaggerOperation(Tags = [ApiTags.SignalSubscriptions])]
    [AllowAnonymous]
    public override async Task<ActionResult<SignalSubscriptionCreatedResponse>> HandleAsync(
        [FromBody] CreateSignalSubscriptionRequest request,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<CreateSignalSubscriptionRequest>
            .With(request)
            .Then(req => new CreateSignalSubscriptionCommand
            {
                UserId = req.UserId,
                SignalSourceId = req.SignalSourceId,
            })
            .Then(command => _mediator.Send(command, cancellationToken))
            .Then(id => new SignalSubscriptionCreatedResponse { Id = id.Value })
            .Match(HandleFailure, Ok);
}
