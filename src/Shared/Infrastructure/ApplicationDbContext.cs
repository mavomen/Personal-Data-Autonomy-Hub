using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PDH.Modules.Activities;
using PDH.Modules.Identity;
using PDH.Shared.Kernel;
using PDH.Shared.Infrastructure.Outbox;
using System.Text.Json;

namespace PDH.Shared.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<OAuthIntegration> OAuthIntegrations => Set<OAuthIntegration>();
    public DbSet<ActivityEvent> ActivityEvents => Set<ActivityEvent>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.HasMany(u => u.OAuthIntegrations)
                  .WithOne()
                  .HasForeignKey(i => i.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OAuthIntegration>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Provider).IsRequired().HasMaxLength(50);
            entity.Property(i => i.EncryptedAccessToken).IsRequired();
            entity.Property(i => i.EncryptedRefreshToken).IsRequired();
            entity.Property(i => i.ExpiresAt).IsRequired();
        });

        modelBuilder.Entity<ActivityEvent>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(500);
            entity.Property(a => a.Timestamp).IsRequired();
            entity.Property(a => a.SourceProvider).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Category)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Type).IsRequired();
            entity.Property(m => m.Data).IsRequired();
            entity.Property(m => m.OccurredOn).IsRequired();
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CaptureDomainEvents();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void CaptureDomainEvents()
    {
        var outboxMessages = new List<OutboxMessage>();
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            var events = entry.Entity.DomainEvents.ToList();
            foreach (var domainEvent in events)
            {
                outboxMessages.Add(new OutboxMessage(
                    domainEvent.GetType().AssemblyQualifiedName!,
                    JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    domainEvent.OccurredOn));
            }
            entry.Entity.ClearDomainEvents();
        }

        if (outboxMessages.Any())
        {
            Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}
