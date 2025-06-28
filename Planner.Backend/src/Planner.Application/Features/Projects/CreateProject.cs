using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Domain.Entities;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.Projects;

public sealed record CreateProject(Guid OwnerId, string Name, string Description): Shared.Messaging.ICommand<Guid>;

internal sealed class CreateProjectHandler(IAppDbContext dbContext) 
    : Shared.Messaging.ICommandHandler<CreateProject, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProject request, CancellationToken ct)
    {
        // If user does not exist, return an error
        if(!await dbContext.Users.AnyAsync(x => x.UserId == request.OwnerId, ct))
        {
            return Result<Guid>.Failure(User.Errors.UserNotFound);
        }
        
        // If project with the same name already exists, return an error
        if(await dbContext.Projects.AnyAsync(x => x.Name == request.Name, ct))
        {
            return Result<Guid>.Failure(Project.Errors.ProjectExists);
        }
        
        Project project = new(request.Name, request.Description, request.OwnerId);
        await dbContext.Projects.AddAsync(project, ct);
        await dbContext.UserProjects.AddAsync(new UserProject(request.OwnerId, project.ProjectId), ct);
 
        return Result<Guid>.Success(project.ProjectId);
    }
}