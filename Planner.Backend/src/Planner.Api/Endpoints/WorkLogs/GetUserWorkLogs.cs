using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Planner.Api.Extensions;
using Planner.Application.Features.WorkLogs;
using Planner.Shared.Wrappers.Result;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Planner.Api.Endpoints.WorkLogs;

internal sealed record GetUserWorkLogsRequest(Guid? UserId, DateOnly Date);

internal sealed class GetUserWorkLogsEndpoint(ISender sender) 
    : Endpoint<GetUserWorkLogsRequest, Results<Ok<List<WorkLogsDetails>>, BadRequest<ProblemDetails>>>
{
    public override void Configure()
    {
        Get("");
        Group<WorkLogsGroup>();
    }

    public override async Task<Results<Ok<List<WorkLogsDetails>>, BadRequest<ProblemDetails>>> ExecuteAsync(GetUserWorkLogsRequest req, CancellationToken ct)
    {
        Guid userId = req.UserId ?? User.GetUserId();
        (List<WorkLogsDetails>? userWorkLogs, Error? error) = await sender.Send(new GetUserWorkLogs(userId, req.Date), ct);
        
        return error is null 
            ? TypedResults.Ok(userWorkLogs!)
            : TypedResults.BadRequest(new ProblemDetails
            {
                Title = error.Description,
                Status = (int)error.Code,
            });
    }
}