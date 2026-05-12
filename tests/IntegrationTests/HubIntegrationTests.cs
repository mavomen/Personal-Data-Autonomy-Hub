using Microsoft.AspNetCore.SignalR.Client;
using PDH.Api.Hubs;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PDH.Api;

namespace PDH.IntegrationTests;

public class HubIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public HubIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task NotificationHub_Can_Be_Connected()
    {
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Configure anything if needed
            });
        }).CreateClient();

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost/hubs/notifications", options =>
            {
                options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
            })
            .Build();

        await hubConnection.StartAsync();
        Assert.Equal(HubConnectionState.Connected, hubConnection.State);
        await hubConnection.StopAsync();
    }
}
