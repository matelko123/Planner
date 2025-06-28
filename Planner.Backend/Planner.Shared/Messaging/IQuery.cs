using MediatR;
using Planner.Shared.Wrappers.Result;

namespace Planner.Shared.Messaging;

public interface IQuery<TValue> : IRequest<Result<TValue>>;