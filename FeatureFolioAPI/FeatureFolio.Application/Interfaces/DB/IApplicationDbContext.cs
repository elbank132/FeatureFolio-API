using FeatureFolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureFolio.Application.Interfaces.DB;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Event> Events { get; }
    DbSet<Cluster> Clusters { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
