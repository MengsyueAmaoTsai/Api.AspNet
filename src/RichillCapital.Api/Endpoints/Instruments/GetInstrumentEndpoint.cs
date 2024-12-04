using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Instruments.Queries;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Instruments;

[ApiVersion(EndpointVersion.V1)]
public sealed class GetInstrumentEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<string>
    .WithActionResult<InstrumentDetailsResponse>
{
    [HttpGet(ApiRoutes.Instruments.Get)]
    [SwaggerOperation(Tags = [ApiTags.Instruments])]
    [AllowAnonymous]
    public override async Task<ActionResult<InstrumentDetailsResponse>> HandleAsync(
        [FromRoute(Name = nameof(symbol))] string symbol,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<string>
            .With(symbol)
            .Then(s => new GetInstrumentQuery { Symbol = s })
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(dto => dto.ToDetailsResponse())
            .Match(HandleFailure, Ok);
}