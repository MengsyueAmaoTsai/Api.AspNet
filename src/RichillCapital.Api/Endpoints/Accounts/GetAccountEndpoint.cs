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
public sealed class GetAccountEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<string>
    .WithActionResult<AccountDetailsResponse>
{
    [HttpGet(ApiRoutes.Accounts.Get)]
    [SwaggerOperation(Tags = [ApiTags.Accounts])]
    [AllowAnonymous]
    public override async Task<ActionResult<AccountDetailsResponse>> HandleAsync(
        [FromRoute(Name = nameof(accountId))] string accountId,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<string>
            .With(accountId)
            .Then(id => new GetAccountQuery { AccountId = id })
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(dto => dto.ToDetailsResponse())
            .Match(HandleFailure, Ok);
}
