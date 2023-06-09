using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("Project");

        builder.Property(_ => _.Id)
            .HasColumnName("ProjectId");

        builder.Property(_ => _.DevOpsProjectName)
            .HasColumnType("nvarchar(256)");
    }
}
