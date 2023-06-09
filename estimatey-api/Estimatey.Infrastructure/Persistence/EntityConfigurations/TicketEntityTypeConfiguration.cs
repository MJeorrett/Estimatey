using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estimatey.Infrastructure.Persistence.EntityConfigurations;

internal class TicketEntityTypeConfiguration : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.ToTable("Ticket");

        builder.Property(_ => _.Id)
            .HasColumnName("TicketId");

        builder.HasIndex(_ => _.DevOpsId)
            .IsUnique();

        builder.Property(_ => _.State)
            .HasMaxLength(16);
    }
}
