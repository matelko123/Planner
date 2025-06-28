using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Domain.Entities;
using Planner.Shared.Messaging;
using Planner.Shared.Wrappers.Result;

namespace Planner.Application.Features.Users;

public sealed record CreateUser(Guid UserId, string FirstName, string LastName, string Email) : ICommand<Guid>;

internal sealed class CreateUserHandler(IAppDbContext dbContext) 
    : ICommandHandler<CreateUser, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUser command, CancellationToken ct)
    {
        if(await dbContext.Users.AnyAsync(x => x.Email == command.Email, ct))
        {
            return Result<Guid>.Failure(User.Errors.UserAlreadyExists);
        }
        
        User user = new(command.UserId, command.FirstName, command.LastName, command.Email);
        await dbContext.Users.AddAsync(user, ct);
        return Result<Guid>.Success(user.UserId);
    }
}