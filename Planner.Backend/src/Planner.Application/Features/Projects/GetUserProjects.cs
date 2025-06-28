using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Shared.Messaging;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.Projects;

public sealed record GetUserProjects(Guid UserId) : IQuery<List<ProjectDetails>>;

public sealed record ProjectDetails(
    Guid ProjectId,
    string Name,
    string Description,
    bool IsOwner,
    int MembersCount);
    
internal sealed class GetUserProjectsHandler(IAppDbContext dbContext) : IQueryHandler<GetUserProjects, List<ProjectDetails>>
{
    public async Task<Result<List<ProjectDetails>>> Handle(GetUserProjects request, CancellationToken ct)
    {
        List<ProjectDetails> projects = await dbContext.Projects
            .Where(p => p.UserProjects.Any(up => up.UserId == request.UserId))
            .Select(p => new ProjectDetails(
                p.ProjectId,
                p.Name,
                p.Description,
                p.OwnerId == request.UserId,
                p.UserProjects.Count(up => up.ProjectId == p.ProjectId)))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result<List<ProjectDetails>>.Success(projects);
    }
}