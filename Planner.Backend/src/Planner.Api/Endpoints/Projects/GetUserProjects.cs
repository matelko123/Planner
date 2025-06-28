using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Api.Extensions;
using Planner.Application.Features.Projects;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.Projects;

internal sealed record GetUserProjectsRequest(Guid? UserId);

internal sealed record GetUserProjectsResponse(List<ProjectDetails> Projects);

internal class GetUserProjectsEndpoint(ISender sender)
    : Endpoint<GetUserProjectsRequest, Results<Ok<GetUserProjectsResponse>, BadRequest<ProblemDetails>>>
{
    public override void Configure()
    {
        Get("");
        Group<ProjectsGroup>();
    }

    public override async Task<Results<Ok<GetUserProjectsResponse>, BadRequest<ProblemDetails>>> ExecuteAsync(GetUserProjectsRequest req, CancellationToken ct)
    {
        Guid userId = req.UserId ?? User.GetUserId();
        Result<List<ProjectDetails>> projects = await sender.Send(new GetUserProjects(userId), ct);
        
        return projects
            ? TypedResults.Ok(new GetUserProjectsResponse(projects.Value!))
            : TypedResults.BadRequest(new ProblemDetails
            {
                Title = projects.Error!.Description,
                Status = (int)projects.Error.Code
            });
    }
}