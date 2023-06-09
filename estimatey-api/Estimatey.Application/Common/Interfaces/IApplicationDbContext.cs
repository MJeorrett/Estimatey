using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<ProjectEntity> Projects { get; }

    public DbSet<FeatureEntity> Features { get; }

    public DbSet<UserStoryEntity> UserStories { get; }

    public DbSet<TicketEntity> Tickets { get; }

    public DbSet<TagEntity> Tags { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
