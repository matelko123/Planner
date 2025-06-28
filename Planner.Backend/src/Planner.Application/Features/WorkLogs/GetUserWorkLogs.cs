using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Shared.Messaging;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.WorkLogs;

public sealed record GetUserWorkLogs(Guid UserId, DateOnly Date) : IQuery<List<WorkLogsDetails>>;

public sealed record WorkLogsDetails(Guid ProjectId, string ProjectName, decimal Hours, string? Description);


internal sealed class GetUserWorkLogsHandler(IAppDbContext dbContext) : IQueryHandler<GetUserWorkLogs, List<WorkLogsDetails>>
{
    public async Task<Result<List<WorkLogsDetails>>> Handle(GetUserWorkLogs request, CancellationToken ct)
    {
        List<WorkLogsDetails> workLogs = await dbContext.WorkLogs
            .Where(wl => wl.UserId == request.UserId && wl.Date == request.Date)
            .Select(wl => new WorkLogsDetails
            (
                wl.ProjectId,
                wl.Project.Name,
                wl.Hours,
                wl.Description
            ))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result<List<WorkLogsDetails>>.Success(workLogs);
    }
}