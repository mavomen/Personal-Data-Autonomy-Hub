using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PDH.Modules.Activities;
using PDH.Shared.Infrastructure;
using PDH.Shared.Kernel;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.Modules.ML;

public class ActivityImportedEventHandler : INotificationHandler<ActivityImportedEvent>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICategoryPredictor _predictor;
    private readonly IMediator _mediator;
    private readonly ILogger<ActivityImportedEventHandler> _logger;

    public ActivityImportedEventHandler(
        ApplicationDbContext dbContext,
        ICategoryPredictor predictor,
        IMediator mediator,
        ILogger<ActivityImportedEventHandler> logger)
    {
        _dbContext = dbContext;
        _predictor = predictor;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(ActivityImportedEvent notification, CancellationToken cancellationToken)
    {
        var activity = await _dbContext.ActivityEvents
            .FirstOrDefaultAsync(a => a.Id == notification.ActivityId, cancellationToken);

        if (activity == null)
        {
            _logger.LogWarning("ActivityImportedEvent for unknown activity {ActivityId}", notification.ActivityId);
            return;
        }

        var predictedCategory = _predictor.PredictCategory(activity.Title, activity.SourceProvider);
        if (Enum.TryParse<ActivityCategory>(predictedCategory, out var category))
        {
            activity.UpdateCategory(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ActivityCategorizedEvent(activity.Id, predictedCategory), cancellationToken);
            _logger.LogInformation("Activity {ActivityId} classified as {Category}", activity.Id, predictedCategory);
        }
        else
        {
            _logger.LogWarning("Predicted category '{Category}' invalid for {ActivityId}", predictedCategory, activity.Id);
        }
    }
}
