using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public abstract class WorkItemEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : WorkItemEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasIndex(_ => _.DevOpsId)
            .IsUnique();

        builder.Property(_ => _.State)
            .HasMaxLength(16);

        builder.Property(_ => _.Iteration)
            .HasMaxLength(100);
    }
}
