using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Accounts;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Accounts.Commands;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Accounts;

[ApiVersion(EndpointVersion.V1)]
public sealed class CreateAccountEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<CreateAccountRequest>
    .WithActionResult<AccountCreatedResponse>
{
    [HttpPost(ApiRoutes.Accounts.Create)]
    [SwaggerOperation(Tags = [ApiTags.Accounts])]
    [AllowAnonymous]
    public override async Task<ActionResult<AccountCreatedResponse>> HandleAsync(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<CreateAccountRequest>
            .With(request)
            .Then(req => new CreateAccountCommand
            {
                UserId = req.UserId,
                Name = req.Name,
            })
            .Then(command => _mediator.Send(command, cancellationToken))
            .Then(id => new AccountCreatedResponse { Id = id.Value })
            .Match(HandleFailure, Ok);
}