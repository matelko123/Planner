using FastEndpoints;

namespace Planner.Api.Endpoints.WorkLogs;

internal sealed class WorkLogsGroup : Group
{
    public WorkLogsGroup()
    {
        Configure("work-logs", _ => {});
    }
}