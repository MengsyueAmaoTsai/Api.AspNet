using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.SignalSources;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.SignalSources.Commands;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.SignalSources;

[ApiVersion(EndpointVersion.V1)]
public sealed class CreateSignalSourceEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<CreateSignalSourceRequest>
    .WithActionResult<SignalSourceCreatedResponse>
{
    [HttpPost(ApiRoutes.SignalSources.Create)]
    [SwaggerOperation(Tags = [ApiTags.SignalSources])]
    [AllowAnonymous]
    public override async Task<ActionResult<SignalSourceCreatedResponse>> HandleAsync(
        [FromBody] CreateSignalSourceRequest request,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<CreateSignalSourceRequest>
            .With(request)
            .Then(req => new CreateSignalSourceCommand
            {
                Id = request.Id,
                Name = req.Name,
                Description = req.Description,
                Version = req.Version,
                Stage = req.Stage,
            })
            .Then(command => _mediator.Send(command, cancellationToken))
            .Then(id => new SignalSourceCreatedResponse { Id = id.Value })
            .Match(HandleFailure, Ok);
}