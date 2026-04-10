using FeatureFolio.Application.Interfaces.DB;
using FeatureFolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureFolio.Infrastructure.DB;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Cluster> Clusters => Set<Cluster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- User Configuration ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.Role).HasConversion<string>();
        });

        // --- Event Configuration ---
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(200);

            // One-to-Many: One User has Many Events
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Events)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // --- Cluster Configuration ---
        modelBuilder.Entity<Cluster>(entity =>
        {
            entity.HasKey(e => e.ClusterId);

            // One-to-Many: One Event has Many Clusters
            entity.HasOne(e => e.Event)
                  .WithMany(ev => ev.Clusters)
                  .HasForeignKey(e => e.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
