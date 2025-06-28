using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Domain.Entities;
using Planner.Shared.Messaging;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.WorkLogs;

public sealed record CreateWorkLog(Guid UserId, Guid ProjectId, DateOnly Date, decimal Hours, string? Description) : ICommand;

internal sealed class CreateWorkLogHandler(
    ISender sender, 
    IAppDbContext dbContext
    ) : ICommandHandler<CreateWorkLog>
{
    public async Task<Result> Handle(CreateWorkLog request, CancellationToken ct)
    {
        if (!await dbContext.Users.AnyAsync(u => u.UserId == request.UserId, ct))
        {
            return Result.Failure(User.Errors.UserNotFound);
        }
        
        if (!await dbContext.Projects.AnyAsync(p => p.ProjectId == request.ProjectId, ct))
        {
            return Result.Failure(Project.Errors.ProjectNotFound);
        }
        
        if (!await dbContext.UserProjects.AnyAsync(up => up.UserId == request.UserId && up.ProjectId == request.ProjectId, ct))
        {
            return Result.Failure(UserProject.Errors.UserNotInProject);
        }
        
        if (await dbContext.WorkLogs.AnyAsync(wl => wl.UserId == request.UserId && wl.ProjectId == request.ProjectId && wl.Date == request.Date, ct))
        {
            return Result.Failure(WorkLog.Errors.WorkLogAlreadyExists);
        }
        
        (List<WorkLogsDetails>? userWorkLogs, Error? error) = await sender.Send(new GetUserWorkLogs(request.UserId, request.Date), ct);
        if (error is not null)
        {
            return Result.Failure(error);
        }
        
        if (userWorkLogs!.Sum(wl => wl.Hours) + request.Hours > 24)
        {
            return Result.Failure(WorkLog.Errors.WorkLogTooManyHours);
        }
        
        WorkLog workLog = new(request.UserId, request.ProjectId, request.Date, request.Hours, request.Description);
        await dbContext.WorkLogs.AddAsync(workLog, ct);
        return Result.Success();
    }
}