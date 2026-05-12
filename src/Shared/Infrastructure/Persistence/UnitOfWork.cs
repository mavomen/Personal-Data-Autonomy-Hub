using PDH.Shared.Infrastructure.DomainEvents;

namespace PDH.Shared.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    {
        _dbContext = dbContext;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Domain events will be dispatched AFTER save by the interceptor
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return result;
    }
}
