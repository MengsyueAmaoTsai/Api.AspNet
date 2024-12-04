using Asp.Versioning;

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
public sealed class ListUsersEndpoint(
    IMediator _mediator) : AsyncEndpoint
    .WithoutRequest
    .WithActionResult<IEnumerable<UserResponse>>
{
    [HttpGet(ApiRoutes.Users.List)]
    [SwaggerOperation(Tags = [ApiTags.Users])]
    [AllowAnonymous]
    public override async Task<ActionResult<IEnumerable<UserResponse>>> HandleAsync(
        CancellationToken cancellationToken = default) =>
        await ErrorOr<ListUsersQuery>
            .With(new ListUsersQuery())
            .Then(query => _mediator.Send(query, cancellationToken))
            .Then(users => users
                .Select(u => u.ToResponse())
                .ToList())
            .Match(HandleFailure, Ok);
}
