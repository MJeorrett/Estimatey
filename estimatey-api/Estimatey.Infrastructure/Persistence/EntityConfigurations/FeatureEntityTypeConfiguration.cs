using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class FeatureEntityTypeConfiguration : IEntityTypeConfiguration<FeatureEntity>
{
    public void Configure(EntityTypeBuilder<FeatureEntity> builder)
    {
        builder.ToTable("Feature");

        builder.Property(_ => _.Id)
            .HasColumnName("FeatureId");

        builder.HasIndex(_ => _.DevOpsId)
            .IsUnique();

        builder.Property(_ => _.State)
            .HasMaxLength(16);
    }
}
