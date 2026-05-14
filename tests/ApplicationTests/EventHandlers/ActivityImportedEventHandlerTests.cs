using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using PDH.Modules.Activities;
using PDH.Modules.ML;
using PDH.Shared.Infrastructure;
using PDH.Shared.Kernel;
using PDH.Shared.Kernel.Interfaces;
using Xunit;

namespace PDH.ApplicationTests.EventHandlers;

public class ActivityImportedEventHandlerTests
{
    [Fact]
    public async Task Handle_Classifies_Activity_And_Publishes_Event()
    {
        var activity = new ActivityEvent(Guid.NewGuid(), "Coding", DateTime.UtcNow, "GitHub");
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new ApplicationDbContext(dbContextOptions);
        dbContext.ActivityEvents.Add(activity);
        await dbContext.SaveChangesAsync();

        var predictorMock = new Mock<ICategoryPredictor>();
        predictorMock.Setup(p => p.PredictCategory("Coding", "GitHub")).Returns("DeepWork");
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<ActivityImportedEventHandler>>();

        var handler = new ActivityImportedEventHandler(dbContext, predictorMock.Object, mediatorMock.Object, loggerMock.Object);
        var evt = new ActivityImportedEvent(activity.Id);

        await handler.Handle(evt, CancellationToken.None);

        Assert.Equal(ActivityCategory.DeepWork, activity.Category);
        mediatorMock.Verify(m => m.Publish(It.IsAny<ActivityCategorizedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
