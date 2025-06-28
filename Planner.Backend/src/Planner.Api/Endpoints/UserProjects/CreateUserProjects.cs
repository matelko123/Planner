using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Application.Data;
using Planner.Application.Features.UserProjects;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.UserProjects;

internal record Request(Guid UserId, Guid ProjectId);

internal sealed class CreateUserProjectsEndpoint(
    ISender sender, 
    IAppDbContext dbContext
    ) : Endpoint<Request, Results<Ok, BadRequest<ProblemDetails>>>
{
    public override void Configure()
    {
        Post("");
        Group<UserProjectsGroup>();
        Description(x => x
            .WithSummary("Create user project")
            .WithDescription("Adding user to project")
            .Produces<Ok>()
            .Produces<BadRequest<ProblemDetails>>());
    }

    public override async Task<Results<Ok, BadRequest<ProblemDetails>>> ExecuteAsync(Request req, CancellationToken ct)
    {
        Result userProject = await sender.Send(new CreateUserProjects(req.UserId, req.ProjectId), ct);
        if (!userProject)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = userProject.Error!.Description,
                Status = (int)userProject.Error.Code
            });
        }
        
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }
}