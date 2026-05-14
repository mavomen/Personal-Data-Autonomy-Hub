using Microsoft.AspNetCore.Http;
using Moq;
using PDH.Api.Services;
using System.Security.Claims;
using Xunit;

namespace PDH.ApplicationTests.Services;

public class CurrentUserServiceTests
{
    [Fact]
    public void UserId_Returns_Claim_Value()
    {
        var expectedId = Guid.NewGuid().ToString();
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, expectedId),
            new Claim(ClaimTypes.Email, "test@test.com")
        }));
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(context);
        var service = new CurrentUserService(httpContextAccessorMock.Object);

        var userId = service.UserId;
        Assert.Equal(expectedId, userId.ToString());
        Assert.Equal("test@test.com", service.Email);
    }
}
