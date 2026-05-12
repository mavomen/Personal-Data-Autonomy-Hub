using Microsoft.EntityFrameworkCore;
using PDH.Modules.Activities;
using PDH.Modules.Identity;

namespace PDH.Shared.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<OAuthIntegration> OAuthIntegrations => Set<OAuthIntegration>();
    public DbSet<ActivityEvent> ActivityEvents => Set<ActivityEvent>();

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
    }
}
