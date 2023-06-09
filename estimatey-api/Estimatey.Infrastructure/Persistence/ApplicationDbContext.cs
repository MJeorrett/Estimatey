using Estimatey.Application.Common.Interfaces;
using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Estimatey.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<ProjectEntity> Projects { get; init; }

    public DbSet<FeatureEntity> Features { get; init; }

    public DbSet<UserStoryEntity> UserStories { get; init; }

    public DbSet<TicketEntity> Tickets { get; init; }

    public DbSet<TagEntity> Tags { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
