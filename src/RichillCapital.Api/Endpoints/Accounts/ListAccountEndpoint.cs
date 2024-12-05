using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Accounts;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Accounts.Queries;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Accounts;

[ApiVersion(EndpointVersion.V1)]
public sealed class ListAccountEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult<IEnumerable<AccountResponse>>
{
    [HttpGet(ApiRoutes.Accounts.List)]
    [SwaggerOperation(Tags = [ApiTags.Accounts])]
    [AllowAnonymous]
    public override async Task<ActionResult<IEnumerable<AccountResponse>>> HandleAsync(
        CancellationToken cancellationToken = default) =>
        await ErrorOr<ListAccountsQuery>
            .With(new ListAccountsQuery())
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(accounts => accounts
                .Select(a => a.ToResponse())
                .ToList())
            .Match(HandleFailure, Ok);
}