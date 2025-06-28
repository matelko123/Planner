using MediatR;
using Planner.Shared.Wrappers.Result;

namespace Planner.Shared.Messaging;


public interface ICommand : IRequest<Result>;
public interface ICommand<TValue> : IRequest<Result<TValue>>;