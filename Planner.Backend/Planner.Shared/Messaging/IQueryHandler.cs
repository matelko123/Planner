using MediatR;
using Planner.Shared.Wrappers.Result;

namespace Planner.Shared.Messaging;

public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue>> 
    where TQuery : IQuery<TValue>;