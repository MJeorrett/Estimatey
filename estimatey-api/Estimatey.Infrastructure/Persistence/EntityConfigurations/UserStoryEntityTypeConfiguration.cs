using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class UserStoryEntityTypeConfiguration : WorkItemEntityTypeConfiguration<UserStoryEntity>
{
    public override void Configure(EntityTypeBuilder<UserStoryEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserStory");

        builder.Property(_ => _.Id)
            .HasColumnName("UserStoryId");
    }
}
