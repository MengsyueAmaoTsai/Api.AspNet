using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Orders;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Orders.Queries;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Orders;

[ApiVersion(EndpointVersion.V1)]
public sealed class ListOrdersEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult<IEnumerable<OrderResponse>>
{
    [HttpGet(ApiRoutes.Orders.List)]
    [SwaggerOperation(Tags = [ApiTags.Orders])]
    [AllowAnonymous]
    public override async Task<ActionResult<IEnumerable<OrderResponse>>> HandleAsync(
        CancellationToken cancellationToken = default) =>
        await ErrorOr<ListOrdersQuery>
            .With(new ListOrdersQuery())
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(orders => orders
                .Select(o => o.ToResponse())
                .ToList())
            .Match(HandleFailure, Ok);
}