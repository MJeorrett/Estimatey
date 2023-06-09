using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class FeatureTagEntityTypeConfiguration : IEntityTypeConfiguration<FeatureTagEntity>
{
    public void Configure(EntityTypeBuilder<FeatureTagEntity> builder)
    {
        builder.ToTable("FeatureTag");
    }
}
