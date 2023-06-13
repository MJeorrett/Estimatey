using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

public class FloatPersonEntityTypeConfiguration : IEntityTypeConfiguration<FloatPersonEntity>
{
    public void Configure(EntityTypeBuilder<FloatPersonEntity> builder)
    {
        builder.ToTable("FloatPerson");

        builder.Property(_ => _.Id)
            .HasColumnName("FloatPersonId");

        builder.Property(_ => _.Name)
            .HasMaxLength(64);
    }
}
