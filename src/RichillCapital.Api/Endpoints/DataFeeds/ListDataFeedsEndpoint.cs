using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.DataFeeds;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.DataFeeds.Queries;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.DataFeeds;

[ApiVersion(EndpointVersion.V1)]
public sealed class ListDataFeedsEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult<IEnumerable<DataFeedResponse>>
{
    [HttpGet(ApiRoutes.DataFeeds.List)]
    [SwaggerOperation(Tags = [ApiTags.DataFeeds])]
    [AllowAnonymous]
    public override async Task<ActionResult<IEnumerable<DataFeedResponse>>> HandleAsync(
        CancellationToken cancellationToken = default) =>
        await ErrorOr<ListDataFeedsQuery>
            .With(new ListDataFeedsQuery())
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(dataFeeds => dataFeeds
                .Select(df => df.ToResponse())
                .ToList())
            .Match(HandleFailure, Ok);
}