using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class BugTagEntityTypeConfiguration : IEntityTypeConfiguration<BugTagEntity>
{
    public void Configure(EntityTypeBuilder<BugTagEntity> builder)
    {
        builder.ToTable("BugTag");
    }
}
