using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class LoggedTimeEntityTypeConfiguration : IEntityTypeConfiguration<LoggedTimeEntity>
{
    public void Configure(EntityTypeBuilder<LoggedTimeEntity> builder)
    {
        builder.ToTable("LoggedTime");

        builder.Property(_ => _.Id)
            .HasColumnName("LoggedTimeId");
    }
}
