using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Api.Extensions;
using Planner.Application.Data;
using Planner.Application.Features.Users;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.Users;

internal sealed record Response(Guid UserId);

internal sealed class CreateUserEndpoint(
    ISender sender, 
    IAppDbContext dbContext
    ) : EndpointWithoutRequest<Results<Ok<Response>, BadRequest<ProblemDetails>>>
{
    public override void Configure()
    {
        Post("");
        Group<UsersGroup>();
    }

    public override async Task<Results<Ok<Response>, BadRequest<ProblemDetails>>> ExecuteAsync(CancellationToken ct)
    {
        Result<Guid> userId = await sender.Send(new CreateUser(User.GetUserId(), User.GetUserFirstName(), User.GetUserLastName(), User.GetUserEmail()), ct);
        if (!userId)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = userId.Error!.Description,
                Status = (int)userId.Error.Code
            });
        }
        
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok(new Response(userId.Value));
    }
}