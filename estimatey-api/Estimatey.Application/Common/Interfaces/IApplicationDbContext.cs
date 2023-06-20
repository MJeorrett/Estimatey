using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ProjectEntity> Projects { get; }

    DbSet<FeatureEntity> Features { get; }

    DbSet<UserStoryEntity> UserStories { get; }

    DbSet<TicketEntity> Tickets { get; }

    DbSet<TagEntity> Tags { get; }

    DbSet<FloatPersonEntity> FloatPeople { get; }

    DbSet<LoggedTimeEntity> LoggedTime { get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
