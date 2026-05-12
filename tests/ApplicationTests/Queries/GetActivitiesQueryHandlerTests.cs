using PDH.Application.Queries.GetActivities;
using PDH.Modules.Activities;
using Xunit;

namespace PDH.ApplicationTests.Queries;

public class GetActivitiesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Paginated_Results()
    {
        var dbContext = TestDbContextFactory.Create();
        for (int i = 0; i < 25; i++)
        {
            dbContext.ActivityEvents.Add(new ActivityEvent(Guid.NewGuid(), $"Activity {i}", DateTime.UtcNow.AddMinutes(-i), "GitHub"));
        }
        await dbContext.SaveChangesAsync();

        var handler = new GetActivitiesQueryHandler(dbContext);
        var query = new GetActivitiesQuery(null, 20);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(20, result.Activities.Count);
        Assert.NotNull(result.NextCursor);
    }
}
