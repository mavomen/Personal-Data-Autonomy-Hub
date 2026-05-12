using MediatR;
using Microsoft.EntityFrameworkCore;
using PDH.Shared.Infrastructure;

namespace PDH.Application.Queries.GetActivities;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, GetActivitiesResult>
{
    private readonly ApplicationDbContext _dbContext;

    public GetActivitiesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetActivitiesResult> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        int limit = request.Limit > 0 ? request.Limit : 20;
        var query = _dbContext.ActivityEvents
            .OrderByDescending(a => a.Timestamp)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Cursor))
        {
            // Cursor is the timestamp of the last item returned, we'll use it as filter
            if (DateTime.TryParse(request.Cursor, out var cursorTime))
            {
                query = query.Where(a => a.Timestamp < cursorTime);
            }
        }

        var activities = await query
            .Take(limit)
            .ToListAsync(cancellationToken);

        string? nextCursor = null;
        if (activities.Count == limit)
        {
            nextCursor = activities.Last().Timestamp.ToString("O");
        }

        return new GetActivitiesResult(activities, nextCursor);
    }
}
