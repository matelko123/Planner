using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Domain.Entities;
using Planner.Shared.Messaging;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.UserProjects;

public sealed record CreateUserProjects(Guid UserId, Guid ProjectId) : ICommand;


internal sealed class CreateUserProjectsHandler(IAppDbContext dbContext) 
    : ICommandHandler<CreateUserProjects>
{
    public async Task<Result> Handle(CreateUserProjects request, CancellationToken ct)
    {
        // If user does not exist, return an error
        if (!await dbContext.Users.AnyAsync(x => x.UserId == request.UserId, ct))
        {
            return Result.Failure(User.Errors.UserNotFound);
        }
        
        // If project does not exist, return an error
        if (!await dbContext.Projects.AnyAsync(x => x.ProjectId == request.ProjectId, ct))
        {
            return Result.Failure(Project.Errors.ProjectNotFound);
        }
        
        // If user is already in the project, do nothing
        if (await dbContext.UserProjects.AnyAsync(x => x.UserId == request.UserId && x.ProjectId == request.ProjectId, ct))
        {
            return Result.Failure(UserProject.Errors.UserAlreadyInProject);
        }
        
        await dbContext.UserProjects.AddAsync(new UserProject(request.UserId, request.ProjectId), ct);
        return Result.Success();
    }
}

