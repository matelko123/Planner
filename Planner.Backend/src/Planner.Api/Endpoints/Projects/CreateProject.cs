using System.Security.Claims;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Application.Data;
using Planner.Application.Features.Projects;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.Projects
{
    internal sealed record Request(string Name, string Description);
    internal sealed record Response(Guid ProjectId);

    internal sealed class CreateProjectEndpoint(ISender sender, IAppDbContext dbContext)
        : Endpoint<Request, Results<Ok<Response>, BadRequest<ProblemDetails>>>
    {
        public override void Configure()
        {
            Post("");
            Group<ProjectsGroup>();
        }

        public override async Task<Results<Ok<Response>, BadRequest<ProblemDetails>>> ExecuteAsync(Request req, CancellationToken ct)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid user ID.",
                });
            } 
        
            Result<Guid> projectId = await sender.Send(new CreateProject(userId, req.Name, req.Description), ct);
            if (!projectId)
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = projectId.Error!.Description,
                    Status = (int)projectId.Error.Code
                });
            }

            await dbContext.SaveChangesAsync(ct);
            return TypedResults.Ok(new Response(projectId.Value));
        }
    }
}