using Moq;
using PDH.Application.Interfaces;
using PDH.Modules.Integrations;
using Xunit;

namespace PDH.ApplicationTests.Integrations;

public class IntegrationFactoryTests
{
    [Fact]
    public void GetService_Returns_Correct_Integration()
    {
        var gitHubMock = new Mock<IIntegrationService>();
        gitHubMock.Setup(s => s.Provider).Returns("GitHub");
        var calendarMock = new Mock<IIntegrationService>();
        calendarMock.Setup(s => s.Provider).Returns("GoogleCalendar");

        var factory = new IntegrationFactory(new[] { gitHubMock.Object, calendarMock.Object });

        Assert.Same(gitHubMock.Object, factory.GetService("GitHub"));
        Assert.Same(calendarMock.Object, factory.GetService("GoogleCalendar"));
    }

    [Fact]
    public void GetService_Unknown_Provider_Throws()
    {
        var factory = new IntegrationFactory(Enumerable.Empty<IIntegrationService>());
        Assert.Throws<ArgumentException>(() => factory.GetService("Unknown"));
    }
}
