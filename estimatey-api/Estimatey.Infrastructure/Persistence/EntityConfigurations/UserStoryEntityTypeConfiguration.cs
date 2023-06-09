using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class UserStoryEntityTypeConfiguration : IEntityTypeConfiguration<UserStoryEntity>
{
    public void Configure(EntityTypeBuilder<UserStoryEntity> builder)
    {
        builder.ToTable("UserStory");

        builder.Property(_ => _.Id)
            .HasColumnName("UserStoryId");

        builder.HasIndex(_ => _.DevOpsId)
            .IsUnique();

        builder.Property(_ => _.State)
            .HasMaxLength(16);
    }
}
