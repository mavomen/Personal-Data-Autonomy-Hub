using PDH.Application.Commands.UpdateActivityCategory;
using PDH.Modules.Activities;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace PDH.ApplicationTests.Commands;

public class UpdateActivityCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Valid_Category_Updates_Entity()
    {
        var activity = new ActivityEvent(Guid.NewGuid(), "Test", DateTime.UtcNow, "GitHub");
        var repoMock = new Mock<IRepository<ActivityEvent>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activity);
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new UpdateActivityCategoryCommandHandler(repoMock.Object, uowMock.Object);
        var command = new UpdateActivityCategoryCommand(activity.Id, "Social");

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal(ActivityCategory.Social, activity.Category);
        repoMock.Verify(r => r.Update(activity), Times.Once);
        uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Invalid_Category_Throws()
    {
        var activity = new ActivityEvent(Guid.NewGuid(), "Test", DateTime.UtcNow, "GitHub");
        var repoMock = new Mock<IRepository<ActivityEvent>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activity);
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new UpdateActivityCategoryCommandHandler(repoMock.Object, uowMock.Object);
        var command = new UpdateActivityCategoryCommand(activity.Id, "Invalid");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
