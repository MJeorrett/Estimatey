using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class FeatureEntityTypeConfiguration : WorkItemEntityTypeConfiguration<FeatureEntity>
{
    public override void Configure(EntityTypeBuilder<FeatureEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Feature");

        builder.Property(_ => _.Id)
            .HasColumnName("FeatureId");
    }
}
