using Moq;
using PDH.Modules.Identity;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.DomainEvents;
using Moq;
using Xunit;

namespace PDH.ApplicationTests.Repositories;

public class RepositoryIntegrationTests
{
    [Fact]
    public async Task GetByIdAsync_Returns_Entity_If_Exists()
    {
        var dbContext = TestDbContextFactory.Create();
        var user = new User(Guid.NewGuid(), "a@b.com", "hash");
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var repo = new Repository<User>(dbContext);
        var retrieved = await repo.GetByIdAsync(user.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("a@b.com", retrieved.Email);
    }

    [Fact]
    public async Task AddAsync_Adds_Entity()
    {
        var dbContext = TestDbContextFactory.Create();
        var repo = new Repository<User>(dbContext);
        var uow = new UnitOfWork(dbContext, Mock.Of<IDomainEventDispatcher>());

        var user = new User(Guid.NewGuid(), "add@test.com", "hash");
        await repo.AddAsync(user);
        await uow.SaveChangesAsync();

        var exists = await dbContext.Users.FindAsync(user.Id);
        Assert.NotNull(exists);
    }
}
