using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Api.Extensions;
using Planner.Application.Data;
using Planner.Application.Features.WorkLogs;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.WorkLogs;

internal sealed record CreateWorkLogsRequest(Guid ProjectId, DateOnly Date, decimal Hours, string? Description);

internal sealed class CreateWorkLogsEndpoint(
    ISender sender, 
    IAppDbContext dbContext
    ) : Endpoint<CreateWorkLogsRequest, Results<Ok, BadRequest<ProblemDetails>>>
{
    public override void Configure()
    {
        Post("");
        Group<WorkLogsGroup>();
    }

    public override async Task<Results<Ok, BadRequest<ProblemDetails>>> ExecuteAsync(CreateWorkLogsRequest req, CancellationToken ct)
    {
        Result workLog = await sender.Send(new CreateWorkLog(User.GetUserId(), req.ProjectId, req.Date, req.Hours, req.Description), ct);
        if (!workLog)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = workLog.Error!.Description,
                Status = (int)workLog.Error!.Code,
            });
        }
        
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }
}