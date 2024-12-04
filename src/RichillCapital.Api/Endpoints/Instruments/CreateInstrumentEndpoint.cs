using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Instruments;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Instruments.Commands;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Instruments;

[ApiVersion(EndpointVersion.V1)]
public sealed class CreateInstrumentEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<CreateInstrumentRequest>
    .WithActionResult<InstrumentCreatedResponse>
{
    [HttpPost(ApiRoutes.Instruments.Create)]
    [SwaggerOperation(Tags = [ApiTags.Instruments])]
    [AllowAnonymous]
    public override async Task<ActionResult<InstrumentCreatedResponse>> HandleAsync(
        [FromBody] CreateInstrumentRequest request,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<CreateInstrumentRequest>
            .With(request)
            .Then(req => new CreateInstrumentCommand
            {
                Symbol = req.Symbol,
                Description = req.Description,
                Type = req.Type,
            })
            .Then(command => _mediator.Send(command, cancellationToken))
            .Then(id => new InstrumentCreatedResponse { Id = id.Value })
            .Match(HandleFailure, Ok);
}