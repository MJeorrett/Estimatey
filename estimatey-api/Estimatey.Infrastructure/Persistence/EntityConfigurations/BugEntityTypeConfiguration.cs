using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class BugEntityTypeConfiguration : WorkItemEntityTypeConfiguration<BugEntity>
{
    public override void Configure(EntityTypeBuilder<BugEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Bug");

        builder.Property(_ => _.Id)
            .HasColumnName("BugId");
    }
}
