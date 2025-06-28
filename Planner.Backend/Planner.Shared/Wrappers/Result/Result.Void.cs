using System.Net;

namespace Planner.Shared.Wrappers.Result;

public class Result
{
    public Error? Error { get; }

    protected Result(Error? error) => Error = error;

    public static implicit operator bool(Result result) => result.Error is null;

    public static Result Success() => new(null);
    public static Result Failure(Error error) => new(error);
    public static Result Failure(HttpStatusCode code, string description) => Failure(new Error(code, description));
    public static Result BadRequest(string description) => Failure(HttpStatusCode.BadRequest, description);
    public static Result NotFound(string description) => Failure(HttpStatusCode.NotFound, description);
}