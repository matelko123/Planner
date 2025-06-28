using MediatR;
using Planner.Shared.Wrappers.Result;

namespace Planner.Shared.Messaging;


public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> 
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue>> 
    where TCommand : ICommand<TValue>;