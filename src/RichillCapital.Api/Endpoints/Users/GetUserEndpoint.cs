﻿using Asp.Versioning;

using RichillCapital.Api.Contracts;
using RichillCapital.Api.Contracts.Users;
using RichillCapital.Api.Endpoints.Abstractions;
using RichillCapital.UseCases.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.SharedKernel.Monads;

using Swashbuckle.AspNetCore.Annotations;

namespace RichillCapital.Api.Endpoints.Users;

[ApiVersion(EndpointVersion.V1)]
public sealed class GetUserEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithRequest<string>
    .WithActionResult<UserDetailsResponse>
{
    [HttpGet(ApiRoutes.Users.Get)]
    [SwaggerOperation(Tags = [ApiTags.Users])]
    [AllowAnonymous]
    public override async Task<ActionResult<UserDetailsResponse>> HandleAsync(
        [FromRoute(Name = nameof(userId))] string userId,
        CancellationToken cancellationToken = default) =>
        await ErrorOr<string>
            .With(userId)
            .Then(id => new GetUserQuery { UserId = id })
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(user => user.ToResponse())
            .Match(HandleFailure, Ok);
}
