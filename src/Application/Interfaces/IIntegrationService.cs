using PDH.Modules.Activities;

namespace PDH.Application.Interfaces;

public interface IIntegrationService
{
    Task<IReadOnlyList<ActivityEvent>> FetchActivitiesAsync(Guid userId, CancellationToken ct = default);
    string Provider { get; }
}
