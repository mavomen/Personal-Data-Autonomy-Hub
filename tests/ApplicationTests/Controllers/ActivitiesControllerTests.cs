using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PDH.Api.Controllers;
using PDH.Application.Commands.UpdateActivityCategory;
using PDH.Application.Queries.GetActivities;
using PDH.Modules.Activities;
using Xunit;

namespace PDH.ApplicationTests.Controllers;

public class ActivitiesControllerTests
{
    [Fact]
    public async Task Get_Returns_Ok_With_Result()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetActivitiesQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetActivitiesResult(new List<ActivityEvent>(), null));
        var controller = new ActivitiesController(mediatorMock.Object);

        var result = await controller.Get(null);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCategory_Returns_NoContent()
    {
        var mediatorMock = new Mock<IMediator>();
        var controller = new ActivitiesController(mediatorMock.Object);

        var result = await controller.UpdateCategory(Guid.NewGuid(), new UpdateCategoryRequest("Social"));

        Assert.IsType<NoContentResult>(result);
    }
}
