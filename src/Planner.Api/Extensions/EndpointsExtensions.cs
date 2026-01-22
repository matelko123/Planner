namespace Planner.Api.Extensions;

internal interface IGroupEndpoint
{
    RouteGroupBuilder InitializeGroup(IEndpointRouteBuilder app);
}

internal interface IEndpoint<TGroup> where TGroup : IGroupEndpoint
{
    static abstract void Map(RouteGroupBuilder group);
}

internal static class EndpointsExtensions
{
    internal static IEndpointRouteBuilder MapEndpoint<TEndpoint, TGroup>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint<TGroup>
        where TGroup : IGroupEndpoint
    {
        TGroup groupEndpoint = Activator.CreateInstance<TGroup>();
        RouteGroupBuilder group = groupEndpoint.InitializeGroup(app);
        TEndpoint.Map(group);
        return app;
    }
}