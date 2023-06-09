using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class UserStoryTagEntityTypeConfiguration : IEntityTypeConfiguration<UserStoryTagEntity>
{
    public void Configure(EntityTypeBuilder<UserStoryTagEntity> builder)
    {
        builder.ToTable("UserStoryTag");
    }
}
