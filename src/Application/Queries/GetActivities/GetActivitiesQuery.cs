using MediatR;
using PDH.Modules.Activities;

namespace PDH.Application.Queries.GetActivities;

public record GetActivitiesQuery(string? Cursor, int Limit = 20) : IRequest<GetActivitiesResult>;
public record GetActivitiesResult(IReadOnlyList<ActivityEvent> Activities, string? NextCursor);
