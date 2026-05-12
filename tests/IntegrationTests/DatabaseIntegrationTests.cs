using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using PDH.Shared.Infrastructure;
using Xunit;

namespace PDH.IntegrationTests;

public class DatabaseIntegrationTests : IAsyncLifetime
{
    private readonly IContainer _dbContainer;
    private string? _connectionString;

    public DatabaseIntegrationTests()
    {
        _dbContainer = new ContainerBuilder()
            .WithImage("postgres:16")
            .WithEnvironment("POSTGRES_USER", "test_user")
            .WithEnvironment("POSTGRES_PASSWORD", "test_pass")
            .WithEnvironment("POSTGRES_DB", "test_db")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var host = _dbContainer.Hostname;
        var port = _dbContainer.GetMappedPublicPort(5432);
        _connectionString = $"Host={host};Port={port};Database=test_db;Username=test_user;Password=test_pass";
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    [Fact]
    public async Task CanApplyMigrationAndQueryDatabase()
    {
        Assert.NotNull(_connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_connectionString)
            .Options;

        using var context = new ApplicationDbContext(options);
        await context.Database.MigrateAsync();
        var canConnect = await context.Database.CanConnectAsync();
        Assert.True(canConnect);
    }
}
