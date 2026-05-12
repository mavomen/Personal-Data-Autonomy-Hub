using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PDH.Shared.Infrastructure.Outbox;

public class OutboxDispatcher : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(IServiceProvider serviceProvider, ILogger<OutboxDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutbox(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }

    private async Task ProcessOutbox(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var messages = await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.OccurredOn)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonSerializer.Deserialize(message.Data, Type.GetType(message.Type)!);
                if (domainEvent != null)
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(domainEvent, cancellationToken);
                }

                message.MarkProcessed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
